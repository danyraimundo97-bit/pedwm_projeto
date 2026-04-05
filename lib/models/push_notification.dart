/// Payload from SignalR hub event `notification` (matches backend anonymous DTO).
class PushNotification {
  final String id;
  final String userId;
  final int type;
  final String message;

  const PushNotification({
    required this.id,
    required this.userId,
    required this.type,
    required this.message,
  });

  factory PushNotification.fromJson(Map<String, dynamic> json) {
    return PushNotification(
      id: json['id']?.toString() ?? '',
      userId: json['userId']?.toString() ?? '',
      type: (json['type'] as num?)?.toInt() ?? 0,
      message: json['message']?.toString() ?? '',
    );
  }
}
