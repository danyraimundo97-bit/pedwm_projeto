import 'package:path/path.dart' as p;
import 'package:sqflite/sqflite.dart';

import 'db_schema.dart';

class AppDatabase {
  AppDatabase._();

  static final AppDatabase instance = AppDatabase._();
  static const String _dbName = 'pedwm.db';

  Database? _database;

  Future<Database> get database async {
    if (_database != null) return _database!;
    _database = await _open();
    return _database!;
  }

  Future<Database> _open() async {
    final dbDir = await getDatabasesPath();
    final dbPath = p.join(dbDir, _dbName);

    return openDatabase(
      dbPath,
      version: DbSchema.version,
      onConfigure: (db) async {
        await db.execute('PRAGMA foreign_keys = ON');
      },
      onCreate: (db, _) async {
        for (final statement in DbSchema.createStatements) {
          await db.execute(statement);
        }
      },
    );
  }

  Future<void> close() async {
    await _database?.close();
    _database = null;
  }
}
