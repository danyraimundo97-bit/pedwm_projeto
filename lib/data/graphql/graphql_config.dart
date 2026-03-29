
class GraphqlConfig {
  GraphqlConfig._();
  /// Matches default backend profile `http://localhost:5287` + `/graphql`.
  static const String defaultGraphqlUrl = 'http://localhost:5287/graphql';
  static const String graphqlUrl = String.fromEnvironment(
    'GRAPHQL_URL',
    defaultValue: defaultGraphqlUrl,
  );


}
