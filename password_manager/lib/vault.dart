import 'package:flutter/material.dart';

class Vault extends StatefulWidget{
  const Vault({super.key});

  @override
  State<Vault> createState() => _VaultState();
}

class _VaultState extends State<Vault> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Text("Vault"),
      ),
    );
  }
}