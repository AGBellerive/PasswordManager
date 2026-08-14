import 'package:flutter/material.dart';
import '../constants/app_colors.dart';
import 'package:flutter/services.dart';
import './pop_up_snack_bar.dart';

class AccountField extends StatefulWidget {
  const AccountField({
    super.key,
    required this.value,
    required this.isPassword,
    this.isCopyable = true,
  });

  final String value;
  final bool isPassword;

  final dynamic isCopyable;

  @override
  State<AccountField> createState() => _AccountFieldState();
}

class _AccountFieldState extends State<AccountField> {
  bool _isPasswordVisible = false;

  String _passwordMask(String password) {
    return '*' * password.length;
  }

  String _passwordUnmask(String password) {
    return password;
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Expanded(
              child: Visibility(
                visible: widget.isPassword && !_isPasswordVisible,
                replacement: Text(
                  _passwordUnmask(widget.value),
                  style: AppColors.textTheme.copyWith(fontSize: 24),
                  overflow: TextOverflow.ellipsis,
                ),
                child: Text(
                  _passwordMask(widget.value),
                  style: AppColors.textTheme.copyWith(fontSize: 24),
                  overflow: TextOverflow.ellipsis,
                ),
              ),
            ),
            Row(
              children: [
                widget.isPassword
                    ? IconButton(
                        icon: Icon(
                          Icons.remove_red_eye,
                          color: AppColors.textColor,
                        ),
                        onPressed: () {
                          setState(() {
                            _isPasswordVisible = !_isPasswordVisible;
                          });
                        },
                      )
                    : SizedBox.shrink(),
                widget.isCopyable
                    ? IconButton(
                        icon: Icon(Icons.copy, color: AppColors.textColor),
                        onPressed: () {
                          Clipboard.setData(ClipboardData(text: widget.value));
                          PopUpSnackBar.show(context, 'Copied to clipboard');
                        },
                      )
                    : SizedBox.shrink(),
              ],
            ),
          ],
        ),
        SizedBox(height: 8),
      ],
    );
  }
}
