import 'package:flutter/material.dart';
import 'package:password_manager/utils/fileoperations.dart';
import './constants/app_colors.dart';
import 'models/account.dart';
import 'utils/sharedpref.dart';
import 'widgets/account_field.dart';
import 'widgets/pop_up_snack_bar.dart';

class CreateAccountModal extends StatefulWidget {
  const CreateAccountModal({super.key, required this.accountNames});

  final Map<String, String> accountNames;

  @override
  State<CreateAccountModal> createState() => _CreateAccountModalState();
}

class _CreateAccountModalState extends State<CreateAccountModal> {
  late TextEditingController _siteController;
  late TextEditingController _emailController;
  late TextEditingController _usernameController;
  late TextEditingController _passwordController;
  late TextEditingController _otherController;

  @override
  void initState() {
    super.initState();
    _siteController = TextEditingController();
    _emailController = TextEditingController();
    _usernameController = TextEditingController();
    _passwordController = TextEditingController();
    _otherController = TextEditingController();
  }

  Future<void> _saveChanges() async {
    if (widget.accountNames.containsKey(_siteController.text)) {
      PopUpSnackBar.showError(
        context,
        'An account with this site already exists.',
      );
      return;
    }
    if (_siteController.text.trim().isEmpty) {
      PopUpSnackBar.showError(context, 'Site field cannot be empty.');
      return;
    }
    if (_passwordController.text.isEmpty) {
      PopUpSnackBar.showError(context, 'Password field cannot be empty.');
      return;
    }

    final newAccount = Account(
      site: _siteController.text.trim(),
      email: _emailController.text.trim(),
      username: _usernameController.text.trim(),
      password: _passwordController.text,
      other: _otherController.text.trim(),
    );

    final path = await SharedPreferencesUtil.get('passwordFile');
    if (path.isNotEmpty) {
      final success = await FileOperations.createAccount(path, newAccount);
      if (!mounted) return;

      if (success) {
        Navigator.pop(context, newAccount);
      } else {
        PopUpSnackBar.showError(context, 'Failed to create account.');
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Padding(
        padding: EdgeInsets.only(
          bottom: MediaQuery.of(context).viewInsets.bottom,
          left: 16.0,
          right: 16.0,
          top: 8.0,
        ),
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
                  icon: Icon(Icons.check, color: AppColors.textColor),
                  onPressed: () {
                    _saveChanges();
                  },
                ),
              ],
            ),
            const SizedBox(height: 8),
            AccountField(
              controller: _siteController,
              isEditing: true,
              isPassword: false,
              labelText: 'Site*',
            ),
            AccountField(
              controller: _emailController,
              isPassword: false,
              isEditing: true,
              labelText: 'Email',
            ),
            AccountField(
              controller: _usernameController,
              isPassword: false,
              isEditing: true,
              labelText: 'Username',
            ),
            AccountField(
              controller: _passwordController,
              isPassword: false,
              isEditing: true,
              labelText: 'Password*',
            ),
            AccountField(
              controller: _otherController,
              isPassword: false,
              isEditing: true,
              isCopyable: false,
              labelText: 'Other',
            ),
          ],
        ),
      ),
    );
  }
}
