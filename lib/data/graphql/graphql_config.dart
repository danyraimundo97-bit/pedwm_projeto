
class GraphqlConfig {
  GraphqlConfig._();
  /// Matches default backend profile `http://localhost:5287` + `/graphql`.
  static const String defaultGraphqlUrl = 'http://localhost:5287/graphql';
  static const String graphqlUrl = String.fromEnvironment(
    'GRAPHQL_URL',
    defaultValue: defaultGraphqlUrl,
  );

  /// SignalR hub URL derived from [graphqlUrl] (same host/port as the API).
  static String get notificationsHubUrl {
    const suffix = '/graphql';
    final u = graphqlUrl.trim();
    if (u.toLowerCase().endsWith(suffix)) {
      return '${u.substring(0, u.length - suffix.length)}/hubs/notifications';
    }
    return '$u/hubs/notifications';
  }
}
