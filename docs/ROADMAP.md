# Student CLI — Yol Haritası

Öğrenci ekleme, listeleme, arama ve silme yapan, verisini JSON dosyasında tutan konsol uygulaması.

- **Önerilen repo adı:** `student-cli`
- **Teknoloji:** .NET 10 konsol, Spectre.Console, xUnit
- **Kurallar:** `CONTRIBUTING.md` · **Tasarım:** `docs/DESIGN.md`

Her aşama ayrı bir branch ve PR olarak yapılır (`stage/<no>-<kisa-ad>`). Bir aşama ancak tüm maddeleri bitince, build/testler geçince ve ekran görüntüleri eklenince tamamlanmış sayılır. Biten maddeler `[x]` yapılır.

## Aşama 1 — Tamamlama (tek aşama)
- [ ] .NET 7 → .NET 10; proje adı `StudentCli`
- [ ] Kod düzeni: `Program.cs` sadece menü; `StudentService`, `JsonStudentRepository` ayrı sınıflar
- [ ] Veriler `students.json` dosyasında kalıcı; sahte veri sadece ilk açılışta
- [ ] Hatalı girişlerde çökmesin (`int.TryParse`), arama ve güncelleme menüsü eklensin
- [ ] Renkli ve hizalı konsol çıktısı (Spectre.Console tablosu)
- [ ] xUnit testleri + GitHub Actions; terminal GIF'i; README (TR + EN)
