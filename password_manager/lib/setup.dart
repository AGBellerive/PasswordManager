import 'package:flutter/material.dart';
import '../widgets/user_input_box.dart';
import '../widgets/pop_up_snack_bar.dart';
import '../utils/sharedpref.dart';
import './constants/app_colors.dart';
import '../utils/fileoperations.dart';
import 'package:file_picker/file_picker.dart';
import 'dart:io';
import './main.dart';
import 'package:dart_ping/dart_ping.dart';
import 'utils/authenticator.dart';

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
  File passwordFile = File('');
  bool canBiometricLock = false;
  bool fromSharedPreferences = false;

  @override
  void initState() {
    super.initState();
    _masterPasswordController.addListener(_onFieldChanged);
    _urlController.addListener(_onFieldChanged);
    _checkBiometricSupport();
    _populateFields();
  }

  @override
  void dispose() {
    _masterPasswordController.removeListener(_onFieldChanged);
    _urlController.removeListener(_onFieldChanged);
    _masterPasswordController.dispose();
    _hintController.dispose();
    _urlController.dispose();
    super.dispose();
  }

  void _onFieldChanged() {
    setState(() {});
  }

  void _populateFields() async {
    String masterPasswordFromShared = await SharedPreferencesUtil.get(
      'masterPassword',
    );
    String hintFromShared = await SharedPreferencesUtil.get('hint');
    String biometricLockFromShared = await SharedPreferencesUtil.get(
      'biometricLock',
    );
    String passwordFilePathFromShared = await SharedPreferencesUtil.get(
      'passwordFile',
    );
    String remoteUrlFromShared = await SharedPreferencesUtil.get('remoteUrl');

    if (!mounted) return;

    setState(() {
      fromSharedPreferences = masterPasswordFromShared.isNotEmpty;
      _masterPasswordController.text = masterPasswordFromShared;
      _hintController.text = hintFromShared;
      biometricLock = biometricLockFromShared.toLowerCase() == 'true';
      if (passwordFilePathFromShared.isNotEmpty) {
        passwordFile = File(passwordFilePathFromShared);
      }
      _urlController.text = remoteUrlFromShared;
      if (remoteUrlFromShared.isNotEmpty) {
        _storageOption = 2;
        localSelected = false;
      } else if (passwordFilePathFromShared.isNotEmpty) {
        _storageOption = 1;
        localSelected = true;
      }
    });
  }

  bool _enableSetUpButton() {
    final hasPassword = _masterPasswordController.text.isNotEmpty;
    if (!hasPassword || _storageOption == null) return false;
    if (_storageOption == 1) {
      return passwordFile.path.isNotEmpty;
    } else {
      return _urlController.text.isNotEmpty;
    }
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

    if (Navigator.canPop(context)) {
      Navigator.pop(context, true);
    } else {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(
          builder: (context) => const MyHomePage(title: 'Home Page'),
        ),
      );
    }
  }

  void _storageOptionPressed(int? value) {
    _storageOption = value;
    localSelected = (_storageOption == 1);
  }

  Widget _localStorageOptions() {
    return Column(
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

              await FileOperations.readAccountFile(file.path);
              if (!mounted) return;
              setState(() {
                passwordFile = file;
              });
              PopUpSnackBar.show(context, 'File selected: ${file.path}');
            }
          },
          child: const Text("Browse File System"),
        ),
        const SizedBox(height: 8),
        Text(
          passwordFile.path.contains('\\')
              ? passwordFile.path.split('\\').last
              : passwordFile.path.split('/').last,
          style: AppColors.textTheme,
        ),
        const SizedBox(height: 16),
      ],
    );
  }

  Widget _remoteStorageOptions() {
    return Column(
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
        const SizedBox(height: 16),
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
            if (!mounted) return;
            setState(() {
              if (event is PingResponse) {
                PopUpSnackBar.show(context, 'Success');
                connectionSuccess = true;
              } else {
                PopUpSnackBar.showError(context, 'Failed to reach remote URL');
                connectionSuccess = false;
              }
            });
          },
          style: ButtonStyle(
            backgroundColor: WidgetStatePropertyAll(
              connectionSuccess ? Colors.green : AppColors.blueAccent,
            ),
          ),
          child: const Text("Test Remote"),
        ),
        const SizedBox(height: 16),
        const Text('Remote URL'),
      ],
    );
  }

  void _checkBiometricSupport() async {
    final authenticator = Authenticator();
    final isSupported = await authenticator.biometricSupport();
    if (!mounted) return;
    setState(() {
      canBiometricLock = isSupported;
    });
  }

  void _onBiometricToggle(bool value) async {
    if (!value) {
      setState(() {
        biometricLock = false;
      });
      return;
    }

    final authenticator = Authenticator();
    final authenticated = await authenticator.authenticateBiometric(
      'Authenticate to enable biometric unlock',
    );
    if (!mounted) return;

    setState(() {
      biometricLock = authenticated;
    });

    if (authenticated) {
      PopUpSnackBar.show(context, 'Biometric unlock enabled');
    } else {
      PopUpSnackBar.showError(context, 'Authentication failed');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundColor,
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              'Set Up Your Vault',
              style: AppColors.textTheme.copyWith(fontSize: 24),
            ),
            const SizedBox(height: 16),
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
            const SizedBox(height: 16),
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

            if (canBiometricLock)
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text("Biometric Unlock", style: AppColors.textTheme),
                  const SizedBox(width: 16),
                  Switch(
                    value: biometricLock,
                    activeThumbColor: Colors.green,
                    activeTrackColor: Colors.white,
                    inactiveThumbColor: Colors.white,
                    inactiveTrackColor: Colors.grey,
                    onChanged: _onBiometricToggle,
                  ),
                ],
              )
            else
              const SizedBox(height: 16),

            Text(
              "Choose your password storage option",
              style: AppColors.textTheme,
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: 350,
              child: RadioGroup<int>(
                groupValue: _storageOption,
                onChanged: (value) {
                  setState(() {
                    _storageOptionPressed(value);
                  });
                },
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const Radio<int>(
                      value: 1,
                      fillColor: WidgetStatePropertyAll(
                        AppColors.blueAccent,
                      ),
                    ),
                    Text('Local Storage', style: AppColors.textTheme),
                    const Radio<int>(
                      value: 2,
                      fillColor: WidgetStatePropertyAll(
                        AppColors.blueAccent,
                      ),
                    ),
                    Text('Remote Storage', style: AppColors.textTheme),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),
            _storageOption == 1
                ? _localStorageOptions()
                : _remoteStorageOptions(),

            SizedBox(
              width: 300,
              child: ElevatedButton(
                onPressed: _enableSetUpButton()
                    ? _setupPressed
                    : () {
                        PopUpSnackBar.show(
                          context,
                          "All Fields Must be entered",
                        );
                      },
                child: fromSharedPreferences ? const Text('Save') : const Text('Setup'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
