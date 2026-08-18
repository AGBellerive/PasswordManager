import 'package:flutter/material.dart';
import '../constants/app_colors.dart';
import 'package:flutter/services.dart';
import './pop_up_snack_bar.dart';

class AccountField extends StatefulWidget {
  const AccountField({
    super.key,
    required this.controller,
    required this.isPassword,
    this.isCopyable = true,
    this.isEditing = false,
    this.labelText,
  });

  final TextEditingController controller;
  final bool isPassword;
  final bool isCopyable;
  final bool isEditing;
  final String? labelText;

  @override
  State<AccountField> createState() => _AccountFieldState();
}

class _AccountFieldState extends State<AccountField> {
  bool _isPasswordVisible = false;

  @override
  void didUpdateWidget(covariant AccountField oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.isEditing && !oldWidget.isEditing) {
      _isPasswordVisible = true;
    }
  }

  Widget _appropriateTextField() {
    if (widget.labelText == 'Other') {
      return TextField(
        keyboardType: TextInputType.multiline,
        minLines: 1,
        maxLines: null,
        controller: widget.controller,
        readOnly: !widget.isEditing,
        style: AppColors.textTheme.copyWith(fontSize: 24),
        decoration: InputDecoration(
          border: widget.isEditing
              ? const UnderlineInputBorder(
                  borderSide: BorderSide(color: AppColors.textColor),
                )
              : InputBorder.none,
          isDense: true,
          contentPadding: const EdgeInsets.symmetric(vertical: 8.0),
        ),
      );
    } else {
      return TextField(
        keyboardType: TextInputType.multiline,
        controller: widget.controller,
        readOnly: !widget.isEditing,

        style: AppColors.textTheme.copyWith(fontSize: 24),
        decoration: InputDecoration(
          border: widget.isEditing
              ? const UnderlineInputBorder(
                  borderSide: BorderSide(color: AppColors.textColor),
                )
              : InputBorder.none,
          isDense: true,
          contentPadding: const EdgeInsets.symmetric(vertical: 8.0),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (widget.labelText != null && widget.isEditing)
          Padding(
            padding: const EdgeInsets.only(bottom: 4.0),
            child: Text(
              widget.labelText!,
              style: AppColors.textTheme.copyWith(
                fontSize: 12,
                color: AppColors.textColor.withOpacity(0.6),
              ),
            ),
          ),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Expanded(child: _appropriateTextField()),
            Row(
              children: [
                widget.isPassword
                    ? IconButton(
                        icon: Icon(
                          _isPasswordVisible
                              ? Icons.visibility
                              : Icons.visibility_off,
                          color: AppColors.textColor,
                        ),
                        onPressed: () {
                          setState(() {
                            _isPasswordVisible = !_isPasswordVisible;
                          });
                        },
                      )
                    : const SizedBox.shrink(),
                (widget.isCopyable && !widget.isEditing)
                    ? IconButton(
                        icon: const Icon(
                          Icons.copy,
                          color: AppColors.textColor,
                        ),
                        onPressed: () {
                          Clipboard.setData(
                            ClipboardData(text: widget.controller.text),
                          );
                          PopUpSnackBar.show(context, 'Copied to clipboard');
                        },
                      )
                    : const SizedBox.shrink(),
              ],
            ),
          ],
        ),
        const SizedBox(height: 8),
      ],
    );
  }
}
