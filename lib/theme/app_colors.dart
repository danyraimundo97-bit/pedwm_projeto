import 'package:flutter/material.dart';

/// Central color palette for the app.
/// Use these constants instead of hardcoding hex values.
class AppColors {
  AppColors._();

  // Backgrounds
  static const Color background = Color(0xFF0D0D0D);
  static const Color cardBg = Color(0xFF1A1A1A);
  static const Color cardBgLighter = Color(0xFF242424);

  // Accent (primary actions, selected states, progress bars)
  static const Color accent = Color(0xFF0183FF);
  static const Color accentDark = Color(0xFF1D4ED8);

  // Text
  static const Color textPrimary = Color(0xFFFFFFFF);
  static const Color textSecondary = Color(0xFF9CA3AF);
  static const Color textMuted = Color(0xFF6B7280);

  // Navigation
  static const Color navSelected = Color(0xFFFFFFFF);
  static const Color navUnselected = Color(0xFF9CA3AF);

  // Status / severity
  static const Color success = Color(0xFF22C55E);
  static const Color error = Color(0xFFEF4444);
  static const Color warning = Color(0xFFF97316);
  static const Color caution = Color(0xFFF59E0B);
  static const Color info = Color(0xFF64748B);

  // Utility
  static const Color transparent = Color(0x00000000);
  static const Color cardShadow = Color(0x0D000000);
}
