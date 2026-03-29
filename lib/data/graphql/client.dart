import 'package:flutter/foundation.dart';
import 'package:graphql/client.dart';

Future<void> debugPingGraphql(GraphQLClient client) async {
  const doc = r'''
    query Ping {
      bemVindo
    }
  ''';
  final result = await client.query(QueryOptions(document: gql(doc)));
  debugPrint('GraphQL data: ${result.data}');
  debugPrint('GraphQL exception: ${result.exception}');
}