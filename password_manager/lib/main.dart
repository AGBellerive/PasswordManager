import 'package:flutter/material.dart';
import '../widgets/user_input_box.dart';
import '../widgets/pop_up_snack_bar.dart';
import '../utils/sharedpref.dart';
import './setup.dart';
import './constants/app_colors.dart';
import './vault.dart';
import 'package:flutter/services.dart';
import 'utils/authenticator.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  SystemChrome.setPreferredOrientations([
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Password Manager',
      theme: ThemeData(colorScheme: ColorScheme.fromSeed(seedColor: Colors.green)),
      home: const MyHomePage(title: 'Home Page'),
      debugShowCheckedModeBanner: false,
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  final TextEditingController _passwordController = TextEditingController();
  bool _isBiometricEnabled = false;
  bool _isAuthenticating = false;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _initializeAuth();
    });
  }

  @override
  void dispose() {
    _passwordController.dispose();
    super.dispose();
  }

  Future<void> _initializeAuth() async {
    final masterPassword = await SharedPreferencesUtil.get('masterPassword');
    if (!mounted) return;

    if (masterPassword.isEmpty) {
      PopUpSnackBar.show(
        context,
        'No master password set. Redirecting to setup...',
      );
      await Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => const SetupPage()),
      );
      if (!mounted) return;
      _initializeAuth();
      return;
    }

    final biometricLockStr = await SharedPreferencesUtil.get('biometricLock');
    final biometricLockEnabled = biometricLockStr.toLowerCase() == 'true';
    final authenticator = Authenticator();
    final isSupported = await authenticator.biometricSupport();

    if (!mounted) return;
    setState(() {
      _isBiometricEnabled = biometricLockEnabled && isSupported;
    });

    // if (_isBiometricEnabled) {
    //   _authenticate();
    // }
  }

  void _infoPressed() async {
    final hint = await SharedPreferencesUtil.get('hint');
    if (!mounted) return;
    if (hint.isNotEmpty) {
      PopUpSnackBar.show(context, 'Password Hint: $hint');
    } else {
      PopUpSnackBar.show(context, 'No password hint set.');
    }
  }

  void _unlockPressed() async {
    final masterHash = await SharedPreferencesUtil.get('masterPassword');
    if (!mounted) return;

    if (masterHash.isNotEmpty && _passwordController.text == masterHash) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => const Vault()),
      );
    } else {
      PopUpSnackBar.showError(context, 'Incorrect password');
    }
  }

  void _authenticate() async {
    if (_isAuthenticating) return;
    _isAuthenticating = true;

    final authenticator = Authenticator();
    final authenticated = await authenticator.authenticateBiometric(
      'Authenticate to Unlock Vault',
    );

    if (!mounted) return;
    _isAuthenticating = false;

    if (authenticated) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => const Vault()),
      );
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
              'Vault Locked',
              style: AppColors.textTheme.copyWith(fontSize: 24),
            ),
            Text(
              'Enter your master password to unlock',
              style: AppColors.textTheme.copyWith(fontSize: 24),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: 300,
              child: UserInputBox(
                hintText: 'Master Password',
                controller: _passwordController,
                isPassword: true,
                icon: Icons.lock_outline,
                keyboardType: TextInputType.text,
                onClick: _unlockPressed,
              ),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: 300,
              child: Row(
                children: [
                  Expanded(
                    child: ElevatedButton(
                      onPressed: _unlockPressed,
                      child: const Text('Unlock'),
                    ),
                  ),
                  if (_isBiometricEnabled) ...[
                    const SizedBox(width: 12),
                    IconButton.filled(
                      onPressed: _authenticate,
                      icon: const Icon(Icons.fingerprint),
                      tooltip: 'Biometric Unlock',
                      style: IconButton.styleFrom(
                        backgroundColor: Colors.green,
                        foregroundColor: Colors.white,
                        padding: const EdgeInsets.all(12),
                      ),
                    ),
                  ],
                ],
              ),
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _infoPressed,
        tooltip: 'Info',
        backgroundColor: AppColors.blueAccent,
        child: const Icon(Icons.info_outline),
      ),
    );
  }
}
