import 'package:graphql/client.dart';
import 'graphql_config.dart';
/// Single app-wide [GraphQLClient] (HTTP, in-memory cache, no auth link).
GraphQLClient createGraphQLClient() {
  final link = HttpLink(GraphqlConfig.graphqlUrl);
  return GraphQLClient(
    cache: GraphQLCache(),
    link: link,
  );
}