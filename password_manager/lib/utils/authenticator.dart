import 'package:flutter/foundation.dart';
import 'package:local_auth/local_auth.dart';

class Authenticator {
  final LocalAuthentication auth = LocalAuthentication();

  Future<bool> biometricSupport() async {
    try {
      final bool canAuthenticateWithBiometrics = await auth.canCheckBiometrics;
      final bool isDeviceSupported = await auth.isDeviceSupported();
      return canAuthenticateWithBiometrics || isDeviceSupported;
    } catch (e) {
      debugPrint('Error checking biometric support: $e');
      return false;
    }
  }

  Future<bool> authenticateBiometric(String msg) async {
    try {
      return await auth.authenticate(
        localizedReason: msg,
        persistAcrossBackgrounding: true,
      );
    } catch (e) {
      debugPrint('Error during biometric authentication: $e');
      return false;
    }
  }
}
