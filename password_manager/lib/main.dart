import 'package:flutter/material.dart';
import '../widgets/user_input_box.dart';
import '../widgets/pop_up_snack_bar.dart';
import '../utils/sharedpref.dart';
import './setup.dart';
import './constants/app_colors.dart';
import './vault.dart';
import 'package:flutter/services.dart';

void main() {
  SystemChrome.setPreferredOrientations([
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Password Manager',
      theme: ThemeData(colorScheme: .fromSeed(seedColor: Colors.green)),
      home: const MyHomePage(title: 'Home Page'),
      debugShowCheckedModeBanner: false,
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  // This widget is the home page of your application. It is stateful, meaning
  // that it has a State object (defined below) that contains fields that affect
  // how it looks.

  // This class is the configuration for the state. It holds the values (in this
  // case the title) provided by the parent (in this case the App widget) and
  // used by the build method of the State. Fields in a Widget subclass are
  // always marked "final".

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  final TextEditingController _passwordController = TextEditingController();

  @override
  void initState() {
    super.initState();

    SharedPreferencesUtil.containsKey('masterPassword').then((value) {
      if (value) {
        _passwordController.text = "";
      } else {
        PopUpSnackBar.show(
          context,
          'No master password set. Redirecting to setup...',
        );
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => SetupPage()),
        );
      }
    });
  }

  void _infoPressed() {
    PopUpSnackBar.show(context, 'Password Hint:\nInfo pressed');
  }

  void _clearSharedPrefs() {
    SharedPreferencesUtil.clear();
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => SetupPage()),
    );
  }

  void _unlockPressed() {
    //read shared prefrence for the masterHash
    //compare the _passwordController.text with the masterHash
    //if match, navigate to home page
    //if not match, show error message
    SharedPreferencesUtil.get('masterPassword').then((masterHash) {
      if (masterHash != null && _passwordController.text == masterHash) {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => Vault()),
        );
      } else {
        PopUpSnackBar.showError(context, 'Incorrect password');
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.

    return Scaffold(
      backgroundColor: AppColors.backgroundColor,
      body: Center(
        child: Column(
          // Invoke "debug painting" -> press "p" in the console to see the  wireframe for each widget.
          mainAxisAlignment: .center,
          children: [
            Text(
              'Vault Locked',
              style: AppColors.textTheme.copyWith(fontSize: 24),
            ),
            Text(
              'Enter your master password to unlock',
              style: AppColors.textTheme.copyWith(fontSize: 24),
            ),
            SizedBox(height: 16),
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
            SizedBox(height: 16),
            SizedBox(
              width: 300,
              child: ElevatedButton(
                onPressed: _unlockPressed,
                child: Text('Unlock'),
              ),
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _infoPressed,
        tooltip: 'Info',
        child: const Icon(Icons.info_outline),
        backgroundColor: AppColors.blueAccent,
      ),
    );
  }
}
