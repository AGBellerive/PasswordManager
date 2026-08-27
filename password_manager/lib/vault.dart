import 'package:floating_action_bubble/floating_action_bubble.dart';
import 'package:flutter/material.dart';
import './constants/app_colors.dart';
import './widgets/user_input_box.dart';
import 'create_account_modal.dart';
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

class _VaultState extends State<Vault> with SingleTickerProviderStateMixin {
  final TextEditingController _searchController = TextEditingController();

  Future<List<Account>> initialFutureAccounts = Future.value([]);
  List<Account> _allAccounts = [];
  List<Account> _filteredAccounts = [];
  bool _isSorted = false;

  late Animation<double> _animation;
  late AnimationController _animationController;

  @override
  void initState() {
    super.initState();
    initialFutureAccounts = _loadAccounts();
    _searchController.addListener(_onSearchChanged);

    _animationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 260),
    );

    final curvedAnimation = CurvedAnimation(
      curve: Curves.easeInOut,
      parent: _animationController,
    );
    _animation = Tween<double>(begin: 0, end: 1).animate(curvedAnimation);
  }

  @override
  void dispose() {
    _searchController.removeListener(_onSearchChanged);
    _searchController.dispose();
    _animationController.dispose();
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

  void _updateAccount(Account oldAccount, Account newAccount) async {
    final index = _allAccounts.indexOf(oldAccount);
    if (index != -1) {
      setState(() {
        _allAccounts[index] = newAccount;
      });
      _onSearchChanged();

      final path = await SharedPreferencesUtil.get('passwordFile');
      if (path != null) {
        await FileOperations.writeAccountFile(path, _allAccounts);
      }
    }
  }

  Future<void> _showAddAccountModal() async {
    var account_names = _allAccounts.map((account) => account.site).toList();

    Map<String, String> accountNames = {
      for (var name in account_names) name: name,
    };

    final newAccount = await showModalBottomSheet<Account>(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (BuildContext context) {
        return Container(
          decoration: const BoxDecoration(
            color: AppColors.cardColor,
            borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
          ),
          child: CreateAccountModal(accountNames: accountNames),
        );
      },
    );

    if (newAccount != null && mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Account created successfully.')),
      );
    }

    _loadAccounts();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundColor,
      body: SafeArea(
        child: Column(
          children: [
            Padding(
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
                      key: const PageStorageKey('vault_list'),
                      clipBehavior: Clip.antiAlias,
                      itemCount: _filteredAccounts.length,
                      itemBuilder: (context, index) {
                        final account = _filteredAccounts[index];
                        return Material(
                          color: AppColors.backgroundColor,
                          child: Padding(
                            padding: const EdgeInsets.fromLTRB(10, 0, 10, 10),
                            child: AccountCard(
                              account: account,
                              onAccountUpdated: (updatedAccount) {
                                _updateAccount(account, updatedAccount);
                              },
                            ),
                          ),
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
      floatingActionButtonLocation: FloatingActionButtonLocation.endDocked,

      //Init Floating Action Bubble
      floatingActionButton: FloatingActionBubble(
        // Menu items
        items: <Bubble>[
          // Floating action menu item
          Bubble(
            title: "Add",
            iconColor: AppColors.cardColor,
            bubbleColor: AppColors.textColor,
            icon: Icons.add,
            titleStyle: TextStyle(fontSize: 16, color: AppColors.cardColor),
            onPress: () {
              _animationController.reverse();
              _showAddAccountModal();
            },
          ),
          Bubble(
            title: "Sort",
            iconColor: AppColors.cardColor,
            bubbleColor: AppColors.textColor,
            icon: _isSorted ? Icons.sort : Icons.sort_by_alpha,
            titleStyle: TextStyle(fontSize: 16, color: AppColors.cardColor),
            onPress: () {
              _animationController.reverse();
              setState(() {
                if (_isSorted) {
                  _allAccounts.sort(
                    (a, b) =>
                        b.site.toLowerCase().compareTo(a.site.toLowerCase()),
                  );
                } else {
                  _allAccounts.sort(
                    (a, b) =>
                        a.site.toLowerCase().compareTo(b.site.toLowerCase()),
                  );
                }
                _isSorted = !_isSorted;
                _filteredAccounts = _allAccounts;
              });
            },
          ),
          Bubble(
            title: "Settings",
            iconColor: AppColors.cardColor,
            bubbleColor: AppColors.textColor,
            icon: Icons.settings,
            titleStyle: TextStyle(fontSize: 16, color: AppColors.cardColor),
            onPress: () {
              _animationController.reverse();
            },
          ),
        ],

        // animation controller
        animation: _animation,

        // On pressed change animation state
        onPress: () => _animationController.isCompleted
            ? _animationController.reverse()
            : _animationController.forward(),

        // Floating Action button Icon color
        iconColor: AppColors.cardColor, // icon color
        // Flaoting Action button Icon
        iconData: Icons.menu,
        backGroundColor: AppColors.textColor, //background color
      ),
    );
  }
}
