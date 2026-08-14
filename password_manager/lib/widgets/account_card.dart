import 'package:flutter/material.dart';
import '../constants/app_colors.dart';
import '../models/account.dart';
import '../account_modal.dart';

class AccountCard extends StatefulWidget {
  const AccountCard({super.key, required this.account});

  final Account account;

  @override
  State<AccountCard> createState() => _AccountCardState();
}

class _AccountCardState extends State<AccountCard> {
  @override
  Widget build(BuildContext context) {
    final account = widget.account;
    return ListTile(
      contentPadding: EdgeInsets.symmetric(horizontal: 20),
      tileColor: AppColors.cardColor,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
      title: Text(
        account.site,
        style: AppColors.textTheme.copyWith(fontSize: 18),
      ),
      subtitle: Text(
        account.email.isNotEmpty ? account.email : account.username,
        style: AppColors.textTheme.copyWith(fontSize: 10),
      ),
      trailing: Icon(Icons.arrow_forward, color: AppColors.textColor),
      onTap: () {
        showModalBottomSheet(
          context: context,
          isScrollControlled: true,
          backgroundColor: AppColors.cardColor,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
          ),
          builder: (BuildContext context) {
            return Container(
              decoration: BoxDecoration(
                color: AppColors.cardColor,
                borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
              ),
              child: AccountModal(clickedAccount: account),
            );
          },
        );
      },
    );
  }
}
