# ![Square44x44Logo scale-125](https://github.com/user-attachments/assets/c8a57c98-42fa-4821-b763-98b9b09bec01) Ina-EarthQuake

Aplikasi Windows berbasis WinUI3 untuk memberikan informasi terkini tentang gempa bumi di Indonesia.

![image](https://github.com/user-attachments/assets/fa1e009e-f36b-4aa7-8aa2-5433990cee5d)

# 📌 Fitur Utama

    ✅ Informasi Gempa Bumi Terbaru: Menampilkan data gempa terbaru di Indonesia.
    ✅ Riwayat Gempa Bumi: Menampilkan data 15 gempa bumi terakhir di Indonesia.
    ✅ Integrasi dengan Open Data BMKG: Mengambil data gempa bumi melalui API yang diberikan oleh BMKG.

# 🚀 Cara Instalasi

Download Assets
Dari halaman Releases:

    Ina-Earthquake_<version>.msix
    Ina-EarthQuake.cer

Install Certificate
Supaya Windows mempercayai package ini buka .cer atau jika tidak bisa ikuti cara di bawah:

    Klik kanan Ina-EarthQuake.cer → Install Certificate
    Pilih Local Machine → Next
    Pilih Place all certificates in the following store → Trusted Root Certification Authorities → Next → Finish
    Jika muncul peringatan, pilih Yes untuk melanjutkan.

Install Aplikasi Untuk menginstall silahkan buka file .msix atau jika tidak bisa ikuti cara di bawah

    Buka PowerShell (boleh tanpa admin) di folder tempat file .msix berada.
    Jalankan:

    Add-AppxPackage .\InaEarthquake-<versi>.msix

        Tunggu proses selesai. Aplikasi akan terpasang seperti biasa, dan bisa dijalankan dari Start Menu.

# ⚠️ Catatan

Pastikan Anda sudah menginstall Microsoft Visual C++ Redistributable terbaru di https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170

# 🪲 Nemu Masalah/Bug?

Jika menemui masalah, cek Issues atau buka issue baru.

# 📚 Kontribusi

Buat Anda yang mau berkontribusi:

    Fork repository ini.
    Buat branch baru (git checkout -b fitur-baru).
    Lakukan perubahan dan commit (git commit -m "Menambahkan fitur baru").
    Push ke branch (git push origin fitur-baru).
    Lakukan Pull Request

# 📜 Lisensi
Proyek ini dilisensikan di bawah MIT License.
