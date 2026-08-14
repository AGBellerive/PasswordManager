import 'package:flutter/material.dart';
import '../constants/app_colors.dart';

class UserInputBox extends StatefulWidget {
  const UserInputBox({
    super.key,
    required this.hintText,
    this.isPassword = false,
    required this.controller,
    this.icon,
    this.keyboardType,
    this.onClick,
  });

  final String hintText;
  final bool isPassword;
  final TextEditingController controller;
  final IconData? icon;
  final TextInputType? keyboardType;
  final VoidCallback? onClick;

  @override
  State<UserInputBox> createState() => _UserInputBoxState();
}

class _UserInputBoxState extends State<UserInputBox> {
  bool isPasswordVisible = false;
  final double cornerRadius = 20.00;

  @override
  Widget build(BuildContext context) {
    return TextFormField(
      controller: widget.controller,
      obscureText: widget.isPassword && !isPasswordVisible,
      keyboardType: widget.keyboardType,
      decoration: InputDecoration(
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(cornerRadius),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(cornerRadius),
          borderSide: BorderSide(color: AppColors.blueAccent, width: 2.0),
        ),
        filled: true,
        fillColor: Colors.white,
        hintText: widget.hintText,
        hintStyle: TextStyle(color: Colors.black54),

        prefixIcon: widget.isPassword
            ? const Icon(Icons.lock_outline, color: Colors.black54)
            : Icon(widget.icon, color: Colors.black54),

        suffixIcon: widget.isPassword
            ? IconButton(
                onPressed: () {
                  setState(() {
                    isPasswordVisible = !isPasswordVisible;
                  });
                },
                icon: Icon(
                  isPasswordVisible
                      ? Icons.visibility
                      : Icons.visibility_off_outlined,
                  color: AppColors.blueAccent,
                ),
              )
            : null,
      ),
      //onTap: widget.onClick,
      onFieldSubmitted: (value) {
        // Handle the submission of the input value here
        widget.onClick?.call();
      },
    );
  }
}
