import 'package:flutter/material.dart';
import './constants/app_colors.dart';
import './widgets/user_input_box.dart';
import 'utils/fileoperations.dart';
import 'models/account.dart';
import 'utils/sharedpref.dart';
import 'widgets/account_card.dart';
import 'account_modal.dart';

class Vault extends StatefulWidget {
  const Vault({super.key});

  @override
  State<Vault> createState() => _VaultState();
}

class _VaultState extends State<Vault> {
  final TextEditingController _searchController = TextEditingController();

  Future<List<Account>> initialFutureAccounts = Future.value([]);
  List<Account> _allAccounts = [];
  List<Account> _filteredAccounts = [];

  @override
  void initState() {
    super.initState();
    initialFutureAccounts = _loadAccounts();
    _searchController.addListener(_onSearchChanged);
  }

  @override
  void dispose() {
    _searchController.removeListener(_onSearchChanged);
    _searchController.dispose();
    super.dispose();
  }

  Future<List<Account>> _loadAccounts() async {
    final path = await SharedPreferencesUtil.get('passwordFile');
    if (path != null) {
      final accounts = await FileOperations.readAccountFile(path);
      _allAccounts = accounts;
      _filteredAccounts = accounts;
      return accounts;
    }
    return <Account>[];
  }

  void _onSearchChanged() {
    final query = _searchController.text.toLowerCase();
    setState(() {
      _filteredAccounts = _allAccounts.where((account) {
        final site = account.site.toLowerCase();
        final email = account.email.toLowerCase();
        final username = account.username.toLowerCase();

        return site.contains(query) ||
            email.contains(query) ||
            username.contains(query);
      }).toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundColor,
      body: SafeArea(
        child: Column(
          children: [
            Material(
              elevation: 0,
              color: AppColors.backgroundColor,
              child: Padding(
                padding: EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'My Vault',
                      style: AppColors.textTheme.copyWith(
                        fontSize: 32,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    SizedBox(height: 20),
                    UserInputBox(
                      hintText: 'Search Account',
                      controller: _searchController,
                      isPassword: false,
                      icon: Icons.search,
                      keyboardType: TextInputType.text,
                    ),
                  ],
                ),
              ),
            ),

            Expanded(
              child: FutureBuilder<List<Account>>(
                future: initialFutureAccounts,
                builder: (context, snapshot) {
                  if (!snapshot.hasData) {
                    return Center(child: CircularProgressIndicator());
                  } else if (snapshot.connectionState ==
                      ConnectionState.waiting) {
                    return Center(child: CircularProgressIndicator());
                  } else if (snapshot.hasError) {
                    return Center(child: Text('Error: ${snapshot.error}'));
                  } else if (snapshot.data!.isEmpty) {
                    return Center(child: Text('No accounts found.'));
                  }
                  if (_filteredAccounts.isEmpty) {
                    return Center(
                      child: Text(
                        _searchController.text.isEmpty
                            ? 'No accounts saved yet.'
                            : 'No accounts match your search.',
                      ),
                    );
                  }

                  return ClipRect(
                    child: ListView.builder(
                      clipBehavior: Clip.hardEdge,
                      itemCount: _filteredAccounts.length,
                      itemBuilder: (context, index) {
                        final account = _filteredAccounts[index];
                        return Padding(
                          padding: EdgeInsets.fromLTRB(10, 0, 10, 10),
                          child: AccountCard(account: account),
                        );
                      },
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {},
        child: Icon(Icons.add),
      ),
    );
  }
}
