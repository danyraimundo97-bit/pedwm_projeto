import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/user_role.dart';
import '../providers/auth_provider.dart';
import '../providers/users_provider.dart';
import '../theme/app_colors.dart';

class CreateUserView extends StatefulWidget {
  const CreateUserView({super.key});

  @override
  State<CreateUserView> createState() => _CreateUserViewState();
}

class _CreateUserViewState extends State<CreateUserView> {
  final _name = TextEditingController();
  final _email = TextEditingController();
  UserRole _role = UserRole.member;

  @override
  void dispose() {
    _name.dispose();
    _email.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();
    if (!auth.canCreateUsers) {
      return Scaffold(
        backgroundColor: AppColors.background,
        appBar: AppBar(backgroundColor: AppColors.background, title: const Text('Create user')),
        body: const Center(child: Text('Only admins can create users.', style: TextStyle(color: AppColors.textMuted))),
      );
    }

    return Scaffold(
      backgroundColor: AppColors.background,
      appBar: AppBar(
        backgroundColor: AppColors.background,
        iconTheme: const IconThemeData(color: AppColors.textPrimary),
        title: const Text('Create user', style: TextStyle(color: AppColors.textPrimary)),
      ),
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextField(
              controller: _name,
              style: const TextStyle(color: AppColors.textPrimary),
              decoration: _decoration('Full name'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _email,
              style: const TextStyle(color: AppColors.textPrimary),
              keyboardType: TextInputType.emailAddress,
              decoration: _decoration('Email'),
            ),
            const SizedBox(height: 16),
            DropdownButtonFormField<UserRole>(
              value: _role,
              dropdownColor: AppColors.cardBg,
              style: const TextStyle(color: AppColors.textPrimary),
              decoration: _decoration('Role'),
              items: UserRole.values
                  .map(
                    (r) => DropdownMenuItem(value: r, child: Text(r.label)),
                  )
                  .toList(),
              onChanged: (v) => setState(() => _role = v ?? UserRole.member),
            ),
            const Spacer(),
            FilledButton(
              onPressed: () async {
                final name = _name.text.trim();
                final email = _email.text.trim();
                if (name.isEmpty || email.isEmpty) return;
                await context.read<UsersProvider>().registerUser(name: name, email: email, role: _role);
                if (context.mounted) Navigator.pop(context);
              },
              child: const Text('Create user'),
            ),
          ],
        ),
      ),
    );
  }

  InputDecoration _decoration(String label) => InputDecoration(
        labelText: label,
        labelStyle: const TextStyle(color: AppColors.textSecondary),
        filled: true,
        fillColor: AppColors.cardBgLighter,
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(12), borderSide: BorderSide.none),
      );
}
