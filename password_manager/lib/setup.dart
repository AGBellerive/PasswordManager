import 'package:flutter/material.dart';
import '../widgets/user_input_box.dart';
import '../widgets/pop_up_snack_bar.dart';
import '../utils/sharedpref.dart';

class SetupPage extends StatefulWidget {
  const SetupPage({super.key});

  @override
  State<SetupPage> createState() => _SetupPageState();
}

class _SetupPageState extends State<SetupPage> {

  final TextEditingController _masterPasswordController = TextEditingController();
  int? _storageOption;
  bool localSelected = false;

  bool _enableSetUpButton(){
    return _masterPasswordController.text.isNotEmpty && _storageOption != null;
  }

  void _setupPressed() {
    PopUpSnackBar.show(context, 'Setup pressed' + _masterPasswordController.text);
  }

  void _storageOptionPressed(int? value){
    _storageOption = value;
    localSelected = (_storageOption == 1);
  }

  _localStorageOptions(){
    return Text("Local storage selected");
    //Browse file system to find the password file
    // attempt to parse the file based on the password file format
    //if successful, store hash in shared pref and navigate
    //if failed, show error
  }

    _remoteStorageOptions(){
    return Text("Remote storage selected");
    //Ask user for a url
    // ping url
    // Attempt to parse the file at the url
    //if successful, store hash password in shared pref and navigate
    //if failed, show error
  }

  

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: .center,
          children: [
            Text('Setup Master Password'),
            SizedBox(height: 16),
            Text('Master Password'),
            SizedBox(height: 8),
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
            Text("Choose your password storage option"),
            SizedBox(height: 16),
            SizedBox(
                width: 300,
                child: Row(
                  mainAxisAlignment: .center,
              children: [
                Radio(value: 1, groupValue: _storageOption, onChanged: (value) {
                  setState(() {
                    _storageOptionPressed(value);
                  });
                }),
                Text('Local Storage'),
                Radio(value: 2, groupValue: _storageOption, onChanged: (value) {
                  setState(() {
                    _storageOptionPressed(value);
                  });
                }),
                Text('Remote Storage'),
              ],
            ),
            ),
            SizedBox(height: 16),
            localSelected ? _localStorageOptions() : _remoteStorageOptions(),

            SizedBox(
              width: 300,
              child: ElevatedButton(
                onPressed: _enableSetUpButton() ? _setupPressed : () => {PopUpSnackBar.show(context, "All Fields Must be entered")},
                child: Text('Setup'),
                
              ),
            ),
          ],
        ),
      ),
    );
  }
}