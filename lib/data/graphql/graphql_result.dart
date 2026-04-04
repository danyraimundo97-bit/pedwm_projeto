import 'package:graphql/client.dart';

/// Throws [Exception] with GraphQL or network error message when the call failed.
void assertNoGraphQlException(QueryResult result) {
  if (result.hasException) {
    final gql = result.exception?.graphqlErrors;
    if (gql != null && gql.isNotEmpty) {
      throw Exception(gql.map((e) => e.message).join('; '));
    }
    throw Exception(result.exception.toString());
  }
}
