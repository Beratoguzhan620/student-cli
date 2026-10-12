# Student CLI — Yol Haritası

Öğrenci ekleme, listeleme, arama ve silme yapan, verisini JSON dosyasında tutan konsol uygulaması.

- **Önerilen repo adı:** `student-cli`
- **Teknoloji:** .NET 10 konsol, System.Text.Json, xUnit
- **Kurallar:** `CONTRIBUTING.md` · **Tasarım:** `docs/DESIGN.md`

Her aşama ayrı bir branch ve PR olarak yapılır (`stage/<no>-<kisa-ad>`). Bir aşama ancak tüm maddeleri bitince, build/testler geçince ve ekran görüntüleri eklenince tamamlanmış sayılır. Biten maddeler `[x]` yapılır.

## Aşama 1 — Tamamlama (tek aşama)
- [x] .NET 7 → .NET 10; proje adı `StudentCli`
- [x] Kod düzeni: `Program.cs` sadece menü; `StudentService`, `JsonStudentRepository` ayrı sınıflar
- [x] Veriler `students.json` dosyasında kalıcı; sahte veri sadece ilk açılışta
- [x] Hatalı girişlerde çökmesin (`int.TryParse`), arama ve güncelleme menüsü eklensin
- [x] Renkli ve hizalı konsol çıktısı (harici paket yerine kendi `ConsoleTable` sınıfı)
- [x] xUnit testleri + GitHub Actions; terminal GIF'i; README (TR + EN)
