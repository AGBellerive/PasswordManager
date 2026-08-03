import 'package:flutter/material.dart';

class Account{
    final String site;
    final String username;
    final String email;
    final String password;
    final String others;

    Account({
        required this.site,
        required this.username,
        required this.email,
        required this.password,
        required this.others,
    });

    factory Account.fromJson(Map<String, dynamic> json) {
        return Account(
            site: json['Site'] ?? '',
            username: json['Username'] ?? '',
            email: json['Email'] ?? '',
            password: json['Password'] ?? '',
            others: json['Others'] ?? '',
        );
    }

    Map<String, dynamic> toJson() {
        return {
            'Site': site,
            'Username': username,
            'Email': email,
            'Password': password,
            'Others': others,
        };
    }
}

