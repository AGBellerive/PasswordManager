import 'package:flutter/material.dart';
import '../widgets/user_input_box.dart';
import '../widgets/pop_up_snack_bar.dart';
import '../utils/sharedpref.dart';
import './constants/app_colors.dart';
import '../utils/fileoperations.dart';
import 'package:file_picker/file_picker.dart';
import 'dart:io';
import '../models/account.dart';
import './main.dart';
import 'package:dart_ping/dart_ping.dart';

class SetupPage extends StatefulWidget {
  const SetupPage({super.key});

  @override
  State<SetupPage> createState() => _SetupPageState();
}

class _SetupPageState extends State<SetupPage> {
  final TextEditingController _masterPasswordController =
      TextEditingController();
  final TextEditingController _hintController = TextEditingController();
  final TextEditingController _urlController = TextEditingController();

  int? _storageOption = 1;
  bool localSelected = false;
  bool biometricLock = false;
  bool connectionSuccess = false;
  File passwordFile = new File('');

  bool _enableSetUpButton() {
    return _masterPasswordController.text.isNotEmpty &&
        _storageOption != null &&
        passwordFile.path.isNotEmpty;
  }

  void _setupPressed() {
    SharedPreferencesUtil.save(
      'masterPassword',
      _masterPasswordController.text,
    );
    SharedPreferencesUtil.save('hint', _hintController.text);
    SharedPreferencesUtil.save('biometricLock', biometricLock.toString());

    if (_storageOption == 1) {
      SharedPreferencesUtil.save('passwordFile', passwordFile.path);
    } else {
      SharedPreferencesUtil.save('remoteUrl', _urlController.text);
    }
    Navigator.push(context, MaterialPageRoute(builder: (context) => MyApp()));
  }

  void _storageOptionPressed(int? value) {
    _storageOption = value;
    localSelected = (_storageOption == 1);
  }

  Widget _localStorageOptions() {
    return (Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        ElevatedButton(
          onPressed: () async {
            PlatformFile? pickedFile = await FilePicker.pickFile(
              type: FileType.custom,
              allowedExtensions: ['json'],
            );

            if (pickedFile != null) {
              final file = File(pickedFile.path!);

              final accounts = await FileOperations.readAccountFile(file.path);
              setState(() {
                passwordFile = file;
              });
              PopUpSnackBar.show(context, 'File selected: ${file.path}');
            }
          },
          child: const Text("Browse File System"),
        ),
        SizedBox(height: 8),
        Text(
          passwordFile.path.contains('\\')
              ? passwordFile.path.split('\\').last
              : passwordFile.path.split('/').last,
          style: AppColors.textTheme,
        ),
        SizedBox(height: 16),
      ],
    ));
  }

  Widget _remoteStorageOptions() {
    return (Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        SizedBox(
          width: 300,
          child: UserInputBox(
            hintText: 'Remote URL',
            controller: _urlController,
            isPassword: false,
            icon: Icons.radar,
            keyboardType: TextInputType.url,
          ),
        ),
        SizedBox(height: 16),
        FilledButton(
          onPressed: () async {
            if (_urlController.text.isEmpty) {
              PopUpSnackBar.show(context, 'Please enter a remote URL');
              return;
            }

            final event = await Ping(
              _urlController.text,
              count: 1,
            ).stream.first;
            setState(() {
              if (event is PingResponse) {
                PopUpSnackBar.show(context, 'Success');
                connectionSuccess = true;
              } else {
                PopUpSnackBar.show(context, 'Failed');
                connectionSuccess = false;
              }
            });
          },
          child: Text("Test Remote"),
          style: ButtonStyle(
            backgroundColor: MaterialStatePropertyAll(
              connectionSuccess ? Colors.green : AppColors.blueAccent,
            ),
          ),
        ),
        SizedBox(height: 16),
        Text('Remote URL'),
      ],
    ));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundColor,
      body: Center(
        child: Column(
          mainAxisAlignment: .center,
          children: [
            Text(
              'Set Up Your Vault',
              style: AppColors.textTheme.copyWith(fontSize: 24),
            ),
            SizedBox(height: 16),
            SizedBox(
              width: 300,
              child: UserInputBox(
                hintText: 'Master Password',
                controller: _masterPasswordController,
                isPassword: true,
                icon: Icons.lock_outline,
                keyboardType: TextInputType.text,
              ),
            ),
            SizedBox(height: 16),
            SizedBox(
              width: 300,
              child: UserInputBox(
                hintText: 'Hint for master password',
                controller: _hintController,
                isPassword: false,
                icon: Icons.question_mark,
                keyboardType: TextInputType.text,
              ),
            ),

            Row(
              mainAxisAlignment: .center,
              children: [
                Text("Biometric Unlock", style: AppColors.textTheme),
                SizedBox(width: 16),
                Switch(
                  value: biometricLock,
                  activeThumbColor: Colors.green,
                  activeTrackColor: Colors.white,
                  inactiveThumbColor: Colors.white,
                  inactiveTrackColor: Colors.grey,
                  onChanged: (bool value) {
                    // This is called when the user toggles the switch.
                    setState(() {
                      biometricLock = value;
                    });
                  },
                ),
              ],
            ),

            Text(
              "Choose your password storage option",
              style: AppColors.textTheme,
            ),
            SizedBox(height: 16),
            SizedBox(
              width: 300,
              child: Row(
                mainAxisAlignment: .center,
                children: [
                  Radio(
                    value: 1,
                    groupValue: _storageOption,
                    fillColor: MaterialStatePropertyAll(AppColors.blueAccent),
                    onChanged: (value) {
                      setState(() {
                        _storageOptionPressed(value);
                      });
                    },
                  ),
                  Text('Local Storage', style: AppColors.textTheme),
                  Radio(
                    value: 2,
                    groupValue: _storageOption,
                    fillColor: MaterialStatePropertyAll(AppColors.blueAccent),
                    onChanged: (value) {
                      setState(() {
                        _storageOptionPressed(value);
                      });
                    },
                  ),
                  Text('Remote Storage', style: AppColors.textTheme),
                ],
              ),
            ),
            SizedBox(height: 16),
            _storageOption == 1
                ? _localStorageOptions()
                : _remoteStorageOptions(),

            SizedBox(
              width: 300,
              child: ElevatedButton(
                onPressed: _enableSetUpButton()
                    ? _setupPressed
                    : () => {
                        PopUpSnackBar.show(
                          context,
                          "All Fields Must be entered",
                        ),
                      },
                child: Text('Setup'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
