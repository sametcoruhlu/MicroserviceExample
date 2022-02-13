Database tarafı için;
Entity Framework ile codefirst olarak oluşturulduğu için;

PostgreSql için bağlantı bilgisi appsettings.json üzerinden ayarlandıktan sonra;

Paket Yöneticisi Konsolu üzerinden;

Add-Migration MicroserviceExample
Update-Database

komutları ile veritabanı migration yapısı ve veritabanı oluşturulması sağlanır.

Daha sonrasında projeyi ayaklandırabilirsiniz.

Örnek Postman denemelerini PostmanCollection altında bulunan dosyası, postman'a import ederek kullanambilirsiniz.