import 'dart:io';
import 'dart:convert';
import '../models/account.dart';

class FileOperations {
  static Future<List<Account>> readAccountFile(String path) async {
    List<Account> accounts = [];
    try {
      final file = File(path);
      if (await file.exists()) {
        final fileContents = await file.readAsString();
        final List<dynamic> jsonList = jsonDecode(fileContents);
        for (var jsonMap in jsonList) {
          accounts.add(Account.fromJson(jsonMap));
        }
        return accounts;
      }
    } catch (e) {
      print('Error reading or parsing file: $e');
    }
    if (accounts.isEmpty) {
      accounts.add(
        Account.fromJson({
          'Site': 'example',
          'Username': 'user',
          'Email': 'user@example.com',
          'Password': 'password',
          'Others': 'info',
        }),
      );
    }
    return accounts;
  }

  static Future<bool> writeAccountFile(
    String path,
    List<Account> accounts,
  ) async {
    try {
      final file = File(path);
      await file.writeAsString(jsonEncode(accounts));
      return true;
    } catch (e) {
      print('Error writing file: $e');
      return false;
    }
  }
}
