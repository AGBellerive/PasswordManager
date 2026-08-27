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
          'Other': 'info',
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
      JsonEncoder encoder = const JsonEncoder.withIndent('  ');
      final file = File(path);
      await file.writeAsString(encoder.convert(accounts));
      return true;
    } catch (e) {
      print('Error writing file: $e');
      return false;
    }
  }

  static Future<bool> updateAccountInFile(
    String path,
    Account oldAccount,
    Account newAccount,
  ) async {
    try {
      final file = File(path);
      if (await file.exists()) {
        final fileContents = await file.readAsString();
        final List<dynamic> jsonList = jsonDecode(fileContents);
        final List<Account> accounts = jsonList
            .map((jsonMap) => Account.fromJson(jsonMap))
            .toList();

        final index = accounts.indexWhere(
          (account) =>
              account.site == oldAccount.site &&
              account.username == oldAccount.username &&
              account.email == oldAccount.email &&
              account.password == oldAccount.password &&
              account.other == oldAccount.other,
        );

        if (index != -1) {
          accounts[index] = newAccount;
          await file.writeAsString(jsonEncode(accounts));
          return true;
        }
      }
    } catch (e) {
      print('Error updating account in file: $e');
    }
    return false;
  }

  static Future<bool> createAccount(String path, Account newAccount) async {
    try {
      final file = File(path);
      List<Account> accounts = [];
      if (await file.exists()) {
        final fileContents = await file.readAsString();
        final List<dynamic> jsonList = jsonDecode(fileContents);
        accounts = jsonList
            .map((jsonMap) => Account.fromJson(jsonMap))
            .toList();
      }
      accounts.add(newAccount);

      JsonEncoder encoder = const JsonEncoder.withIndent('  ');
      await file.writeAsString(encoder.convert(accounts));
      return true;
    } catch (e) {
      print('Error creating account in file: $e');
      return false;
    }
  }

  static Future<bool> deleteAccount(String path, Account account) async {
    try {
      final file = File(path);
      if (await file.exists()) {
        final fileContents = await file.readAsString();
        final List<dynamic> jsonList = jsonDecode(fileContents);
        final List<Account> accounts = jsonList
            .map((jsonMap) => Account.fromJson(jsonMap))
            .toList();

        final index = accounts.indexWhere(
          (acc) =>
              acc.site == account.site &&
              acc.username == account.username &&
              acc.email == account.email &&
              acc.password == account.password &&
              acc.other == account.other,
        );

        if (index != -1) {
          accounts.removeAt(index);
          await file.writeAsString(jsonEncode(accounts));
          return true;
        }
      }
    } catch (e) {
      print('Error deleting account from file: $e');
    }
    return false;
  }
}
