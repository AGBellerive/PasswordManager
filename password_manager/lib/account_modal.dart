import 'package:flutter/material.dart';
import './constants/app_colors.dart';
import 'models/account.dart';
import 'widgets/account_field.dart';

class AccountModal extends StatelessWidget {
  const AccountModal({super.key, required this.clickedAccount});

  final Account clickedAccount;

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
              children: [
                IconButton(
                  icon: const Icon(Icons.close, color: AppColors.textColor),
                  onPressed: () {
                    Navigator.pop(context);
                  },
                ),
              ],
            ),
            const SizedBox(height: 8),
            AccountField(value: clickedAccount.site, isPassword: false),
            clickedAccount.email.isNotEmpty
                ? AccountField(value: clickedAccount.email, isPassword: false)
                : const SizedBox.shrink(),
            clickedAccount.username.isNotEmpty
                ? AccountField(
                    value: clickedAccount.username,
                    isPassword: false,
                  )
                : const SizedBox.shrink(),
            AccountField(value: clickedAccount.password, isPassword: true),
            clickedAccount.others.isNotEmpty
                ? AccountField(
                    value: clickedAccount.others,
                    isPassword: false,
                  )
                : const SizedBox.shrink(),
          ],
        ),
      ),
    );
  }
}
