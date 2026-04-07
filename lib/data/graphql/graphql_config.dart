class GraphqlConfig {
  GraphqlConfig._();

  /// Matches default backend profile `http://localhost:5287` + `/graphql`.
  static const String defaultGraphqlUrl = 'http://localhost:5287/graphql';

  static const String _graphqlUrlDefine = String.fromEnvironment('GRAPHQL_URL');

  static String get graphqlUrl {
    final fromDefine = _graphqlUrlDefine.trim();
    return fromDefine;
  }

  static const String _notificationsHubUrl = String.fromEnvironment(
    'NOTIFICATIONS_HUB_URL',
  );

  static String get notificationsHubUrl {
    final fromDefine = _notificationsHubUrl.trim();
    return fromDefine;
  }
}
