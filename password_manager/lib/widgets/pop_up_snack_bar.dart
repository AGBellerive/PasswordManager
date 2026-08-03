import 'package:flutter/material.dart';

class PopUpSnackBar {
  static void show(BuildContext context, String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message, textAlign: TextAlign.center),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20),
        ),
        backgroundColor: Colors.lightBlueAccent,
        duration: const Duration(seconds: 2),
        width: 250,
      ),
    );
  }
}