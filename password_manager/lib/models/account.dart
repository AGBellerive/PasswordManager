import 'package:flutter/material.dart';

class Account {
  final String site;
  final String username;
  final String email;
  final String password;
  final String other;

  Account({
    required this.site,
    required this.username,
    required this.email,
    required this.password,
    required this.other,
  });

  factory Account.fromJson(Map<String, dynamic> json) {
    return Account(
      site: json['Site'] ?? '',
      username: json['Username'] ?? '',
      email: json['Email'] ?? '',
      password: json['Password'] ?? '',
      other: json['Other'] ?? '',
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'Site': site,
      'Username': username,
      'Email': email,
      'Password': password,
      'Other': other,
    };
  }

  String toString() {
    return 'Account(site: $site, username: $username, email: $email, password: $password, other: $other)';
  }
}
