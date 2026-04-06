import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:signalr_netcore/signalr_client.dart';

import '../data/graphql/graphql_config.dart';
import '../models/push_notification.dart';

/// Connects to the ASP.NET Core SignalR hub and exposes real-time [Notification] payloads.
class NotificationsProvider extends ChangeNotifier {
  HubConnection? _hub;

  final List<PushNotification> _recent = [];
  final StreamController<PushNotification> _events =
      StreamController<PushNotification>.broadcast();

  List<PushNotification> get recent => List.unmodifiable(_recent);

  Stream<PushNotification> get events => _events.stream;

  bool get isConnected => _hub?.state == HubConnectionState.Connected;

  String? connectionError;

  Future<void> connectForUser(String userId) async {
    await disconnect();
    connectionError = null;
    notifyListeners();

    final url = GraphqlConfig.notificationsHubUrl;
    try {
      _hub = HubConnectionBuilder()
          .withUrl(url)
          .withAutomaticReconnect(retryDelays: const [2000, 5000, 10000])
          .build();
      _hub!.on('notification', _onServerNotification);
      _hub!.onclose(({error}) {
        if (kDebugMode) {
          debugPrint('SignalR closed: $error');
        }
        notifyListeners();
      });
      await _hub!.start();
      await _hub!.invoke('JoinUserNotifications', args: <Object>[userId]);
      notifyListeners();
    } catch (e, st) {
      connectionError = e.toString();
      if (kDebugMode) {
        debugPrint('SignalR connect failed: $e\n$st');
      }
      notifyListeners();
    }
  }

  void _onServerNotification(List<Object?>? args) {
    if (args == null || args.isEmpty) {
      return;
    }
    final first = args.first;
    Map<String, dynamic>? map;
    if (first is Map) {
      map = Map<String, dynamic>.from(first);
    }
    if (map == null) {
      return;
    }
    final n = PushNotification.fromJson(map);
    _recent.insert(0, n);
    if (_recent.length > 50) {
      _recent.removeLast();
    }
    if (!_events.isClosed) {
      _events.add(n);
    }
    notifyListeners();
  }

  Future<void> disconnect() async {
    final hub = _hub;
    _hub = null;
    if (hub != null) {
      try {
        await hub.stop();
      } catch (_) {
        // ignore stop errors
      }
    }
    notifyListeners();
  }

  @override
  void dispose() {
    unawaited(disconnect());
    unawaited(_events.close());
    super.dispose();
  }
}
