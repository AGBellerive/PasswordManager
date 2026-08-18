import 'package:flutter/material.dart';
import './constants/app_colors.dart';
import 'models/account.dart';
import 'widgets/account_field.dart';

class AccountModal extends StatefulWidget {
  const AccountModal({super.key, required this.clickedAccount});

  final Account clickedAccount;

  @override
  State<AccountModal> createState() => _AccountModalState();
}

class _AccountModalState extends State<AccountModal> {
  late TextEditingController _siteController;
  late TextEditingController _emailController;
  late TextEditingController _usernameController;
  late TextEditingController _passwordController;
  late TextEditingController _otherController;

  bool _isEditing = false;

  @override
  void initState() {
    super.initState();
    _siteController = TextEditingController(text: widget.clickedAccount.site);
    _emailController = TextEditingController(text: widget.clickedAccount.email);
    _usernameController = TextEditingController(
      text: widget.clickedAccount.username,
    );
    _passwordController = TextEditingController(
      text: widget.clickedAccount.password,
    );
    _otherController = TextEditingController(text: widget.clickedAccount.other);
  }

  @override
  void dispose() {
    _siteController.dispose();
    _emailController.dispose();
    _usernameController.dispose();
    _passwordController.dispose();
    _otherController.dispose();
    super.dispose();
  }

  void _saveChanges() {
    if (_siteController.text.trim().isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Site field cannot be empty.')),
      );
      return;
    }
    if (_passwordController.text.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Password field cannot be empty.')),
      );
      return;
    }

    final updatedAccount = Account(
      site: _siteController.text.trim(),
      email: _emailController.text.trim(),
      username: _usernameController.text.trim(),
      password: _passwordController.text,
      other: _otherController.text.trim(),
    );

    Navigator.pop(context, updatedAccount);
  }

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 8.0),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                IconButton(
                  icon: const Icon(Icons.close, color: AppColors.textColor),
                  onPressed: () {
                    Navigator.pop(context);
                  },
                ),
                IconButton(
                  icon: Icon(
                    _isEditing ? Icons.check : Icons.edit,
                    color: AppColors.textColor,
                  ),
                  onPressed: () {
                    if (_isEditing) {
                      _saveChanges();
                    } else {
                      setState(() {
                        _isEditing = true;
                      });
                    }
                  },
                ),
              ],
            ),
            const SizedBox(height: 8),
            AccountField(
              controller: _siteController,
              isPassword: false,
              isEditing: _isEditing,
              labelText: 'Site',
            ),
            (_isEditing || widget.clickedAccount.email.isNotEmpty)
                ? AccountField(
                    controller: _emailController,
                    isPassword: false,
                    isEditing: _isEditing,
                    labelText: 'Email',
                  )
                : const SizedBox.shrink(),
            (_isEditing || widget.clickedAccount.username.isNotEmpty)
                ? AccountField(
                    controller: _usernameController,
                    isPassword: false,
                    isEditing: _isEditing,
                    labelText: 'Username',
                  )
                : const SizedBox.shrink(),
            AccountField(
              controller: _passwordController,
              isPassword: true,
              isEditing: _isEditing,
              labelText: 'Password',
            ),
            (_isEditing || widget.clickedAccount.other.isNotEmpty)
                ? AccountField(
                    controller: _otherController,
                    isPassword: false,
                    isEditing: _isEditing,
                    isCopyable: false,
                    labelText: 'Other',
                  )
                : const SizedBox.shrink(),
          ],
        ),
      ),
    );
  }
}
