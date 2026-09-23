import 'package:flutter/material.dart';
import '../constants/app_colors.dart';
import '../models/account.dart';
import '../account_modal.dart';

class AccountCard extends StatefulWidget {
  const AccountCard({
    super.key,
    required this.account,
    this.onAccountUpdated,
    this.onAccountDeleted,
  });

  final Account account;
  final ValueChanged<Account>? onAccountUpdated;
  final VoidCallback? onAccountDeleted;

  @override
  State<AccountCard> createState() => _AccountCardState();
}

class _AccountCardState extends State<AccountCard> {
  void _openModal(BuildContext context) async {
    final result = await showModalBottomSheet<dynamic>(
      context: context,
      isScrollControlled: true,
      backgroundColor: AppColors.cardColor,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (BuildContext context) {
        return Container(
          decoration: const BoxDecoration(
            color: AppColors.cardColor,
            borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
          ),
          child: AccountModal(clickedAccount: widget.account),
        );
      },
    );
    if (result == 'deleted') {
      if (widget.onAccountDeleted != null) {
        widget.onAccountDeleted!();
      }
    } else if (result is Account) {
      if (widget.onAccountUpdated != null) {
        widget.onAccountUpdated!(result);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final account = widget.account;
    return ListTile(
      contentPadding: const EdgeInsets.symmetric(horizontal: 20),
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
      trailing: const Icon(Icons.arrow_forward, color: AppColors.textColor),
      onTap: () => _openModal(context),
    );
  }
}
