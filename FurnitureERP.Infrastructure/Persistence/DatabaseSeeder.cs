using FurnitureERP.Domain.Aggregates.Products;
using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Aggregates.Employees;
using FurnitureERP.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SeedAsync()
    {
        var hasMaterials = await _context.Materials.AnyAsync();
        var hasEmployees = await _context.Employees.AnyAsync();
        var hasProducts = await _context.Products.AnyAsync();

        if (hasMaterials || hasEmployees || hasProducts)
        {
            return;
        }

        await SeedMaterialsAsync();
        await SeedEmployeesAsync();
        await _context.SaveChangesAsync();

        await SeedProductsAsync();
        await _context.SaveChangesAsync();

        await SeedOrdersAsync();
        await _context.SaveChangesAsync();
    }

    private async Task SeedMaterialsAsync()
    {
        var materials = new List<Material>
        {
            new Material(
                "Dubová deska 40mm",
                "Masivní dubová deska pro výrobu stolových desek a poliček",
                "Dřevo",
                "m²",
                1850.00m,
                12.5m,
                5.0m,
                "Dřevařský závod Zlín s.r.o."
            ),
            new Material(
                "Bukové latě 20x40mm",
                "Bukové latě pro konstrukci židlí a stolů",
                "Dřevo",
                "bm",
                45.00m,
                150.0m,
                30.0m,
                "Bukovina Trutnov a.s."
            ),
            new Material(
                "Lakované MDF 18mm",
                "Středně hustá vláknitá deska s lakovaným povrchem",
                "Dřevo",
                "m²",
                420.00m,
                25.0m,
                10.0m,
                "MDF Europa Praha"
            ),
            new Material(
                "Překližka březová 12mm",
                "Kvalitní březová překližka pro šuplíky a zádění",
                "Dřevo",
                "m²",
                380.00m,
                18.0m,
                8.0m,
                "Dřevařský závod Zlín s.r.o."
            ),

            new Material(
                "Nerezové šrouby 4x40mm",
                "Šrouby do dřeva z nerezové oceli s plochou hlavou",
                "Kování",
                "ks",
                2.50m,
                500.0m,
                100.0m,
                "Kování Brno spol. s r.o."
            ),
            new Material(
                "Ložiskové panty 110°",
                "Skříňové panty s tlumením pro dvířka",
                "Kování",
                "ks",
                35.00m,
                80.0m,
                20.0m,
                "Kování Brno spol. s r.o."
            ),
            new Material(
                "Nábytkové knoflíky dřevěné",
                "Dubové knoflíky pro šuplíky a dvířka, průměr 35mm",
                "Kování",
                "ks",
                28.00m,
                60.0m,
                15.0m,
                "Dřevěné doplňky Ostrava"
            ),
            new Material(
                "Kuličkové výsuvy 450mm",
                "Plnovýsuvné kolejničky pro šuplíky s nosností 30kg",
                "Kování",
                "pár",
                120.00m,
                40.0m,
                10.0m,
                "Kování Brno spol. s r.o."
            ),

            new Material(
                "Lazura na dřevo - dub",
                "Ochranná lazura pro vnější i vnitřní použití",
                "Nátěrové hmoty",
                "l",
                185.00m,
                15.0m,
                5.0m,
                "Barvy a laky Praha"
            ),
            new Material(
                "Bezbarvý lak na dřevo",
                "Vodou ředitelný akrylátový lak s hedvábným leskem",
                "Nátěrové hmoty",
                "l",
                220.00m,
                12.0m,
                4.0m,
                "Barvy a laky Praha"
            ),
            new Material(
                "Mořidlo mahagon",
                "Vodové mořidlo pro zatmavení dřeva",
                "Nátěrové hmoty",
                "l",
                165.00m,
                8.0m,
                3.0m,
                "Barvy a laky Praha"
            ),

            new Material(
                "Pěnová výplň T25 100mm",
                "Polyuretanová pěna pro sedáky židlí a křesel",
                "Čalounění",
                "m²",
                280.00m,
                20.0m,
                8.0m,
                "Čalounictví Jihlava s.r.o."
            ),
            new Material(
                "Potahová látka - šedá",
                "Odolná potahová látka pro židle a křesla",
                "Čalounění",
                "m",
                420.00m,
                25.0m,
                10.0m,
                "Textil pro nábytek Brno"
            ),
            new Material(
                "Koženka hnědá",
                "Ekologická koženka pro kancelářské židle",
                "Čalounění",
                "m",
                380.00m,
                15.0m,
                5.0m,
                "Textil pro nábytek Brno"
            ),

            new Material(
                "Bezpečnostní sklo 6mm",
                "Kalené sklo pro stolní desky a police",
                "Sklo",
                "m²",
                850.00m,
                8.0m,
                3.0m,
                "Sklo a zrcadla Plzeň"
            ),
            new Material(
                "Zrcadlo 4mm",
                "Obyčejné zrcadlo pro skříně a šuplíky",
                "Sklo",
                "m²",
                420.00m,
                10.0m,
                4.0m,
                "Sklo a zrcadla Plzeň"
            )
        };

        await _context.Materials.AddRangeAsync(materials);
    }

    private async Task SeedEmployeesAsync()
    {
        var employees = new List<Employee>
        {
            new Employee(
                "Jan",
                "Novák",
                "Hlavní truhlář",
                350.00m
            ),
            new Employee(
                "Petr",
                "Svoboda",
                "Truhlář",
                280.00m
            ),
            new Employee(
                "Pavel",
                "Dvořák",
                "Truhlář",
                280.00m
            ),

            new Employee(
                "Martin",
                "Černý",
                "Montážník",
                250.00m
            ),
            new Employee(
                "Tomáš",
                "Procházka",
                "Montážník",
                250.00m
            ),

            new Employee(
                "Josef",
                "Kučera",
                "Lakýrník",
                290.00m
            ),
            new Employee(
                "Jaroslav",
                "Veselý",
                "Lakýrník",
                290.00m
            ),

            new Employee(
                "Marie",
                "Horáková",
                "Čalouník",
                270.00m
            ),
            new Employee(
                "Anna",
                "Němcová",
                "Čalouník",
                270.00m
            ),

            new Employee(
                "Jiří",
                "Marek",
                "Mistr výroby",
                400.00m
            )
        };

        await _context.Employees.AddRangeAsync(employees);
    }

    private async Task SeedProductsAsync()
    {
        var materials = await _context.Materials.ToListAsync();
        var employees = await _context.Employees.ToListAsync();

        var products = new List<Product>();

        var diningTable = new Product(
            "Dubový jídelní stůl 180x90",
            "Masivní dubový jídelní stůl s rozměry 180x90cm, povrchová úprava bezbarvým lakem",
            "Stoly",
            "ST-001",
            0.00m,
            18500.00m
        );
        products.Add(diningTable);

        var kitchenCabinet = new Product(
            "Kuchyňská skříňka 60x60x80",
            "Kuchyňská skříňka s dvěma šuplíky, MDF deska s lakovaným povrchem",
            "Skříně",
            "SK-002",
            0.00m,
            8900.00m
        );
        products.Add(kitchenCabinet);

        var upholsteredChair = new Product(
            "Čalouněná židle Oslo",
            "Moderní čalouněná židle s dubovou konstrukcí a šedou potahovou látkou",
            "Židle",
            "ZI-003",
            0.00m,
            4200.00m
        );
        products.Add(upholsteredChair);

        var bookshelf = new Product(
            "Knihovna Elegance 120x200",
            "Vysoká knihovna s pěti policemi a skleněnými dvířky, dubový dekor",
            "Skříně",
            "SK-004",
            0.00m,
            15800.00m
        );
        products.Add(bookshelf);

        var coffeeTable = new Product(
            "Konferenční stolek Mirror 90x60",
            "Moderní konferenční stolek se zrcadlovou deskou a dubovou konstrukcí",
            "Stoly",
            "ST-005",
            0.00m,
            6500.00m
        );
        products.Add(coffeeTable);

        var officeChair = new Product(
            "Kancelářská židle Executive",
            "Ergonomická kancelářská židle s koženkovým potahem a nastavitelnou výškou",
            "Židle",
            "ZI-006",
            0.00m,
            7800.00m
        );
        products.Add(officeChair);

        var tvStand = new Product(
            "TV stolek Modern 160x45",
            "Moderní TV stolek s šuplíky a otevřenými policemi, dubová konstrukce s bílým lakem",
            "Stoly",
            "ST-007",
            7065.00m,
            12500.00m
        );
        products.Add(tvStand);

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        await AddDiningTableBomAsync(diningTable, materials, employees);
        await AddKitchenCabinetBomAsync(kitchenCabinet, materials, employees);
        await AddUpholsteredChairBomAsync(upholsteredChair, materials, employees);
        await AddBookshelfBomAsync(bookshelf, materials, employees);
        await AddCoffeeTableBomAsync(coffeeTable, materials, employees);
        await AddOfficeChairBomAsync(officeChair, materials, employees);
        await AddTvStandBomAsync(tvStand, materials, employees);

        await _context.SaveChangesAsync();

        foreach (var product in products)
        {
            var basePrice = await CalculateProductBasePriceAsync(product.Id, materials, employees);
            product.UpdateBasePrice(basePrice);
            var salePrice = Math.Ceiling(basePrice * 1.35m / 10) * 10;
            product.UpdateSalePrice(salePrice);
        }

        await _context.SaveChangesAsync();
    }

    private async Task<decimal> CalculateProductBasePriceAsync(int productId, List<Material> materials, List<Employee> employees)
    {
        var materialBoms = await _context.MaterialBoms.Where(mb => mb.ProductId == productId).ToListAsync();
        var laborBoms = await _context.LaborBoms.Where(lb => lb.ProductId == productId).ToListAsync();

        decimal totalMaterialCost = 0;
        foreach (var mb in materialBoms)
        {
            var material = materials.FirstOrDefault(m => m.Id == mb.MaterialId);
            if (material != null)
            {
                totalMaterialCost += mb.QuantityWithWastage * material.PricePerUnit;
            }
        }

        decimal totalLaborCost = 0;
        foreach (var lb in laborBoms)
        {
            var employee = employees.FirstOrDefault(e => e.Id == lb.EmployeeId);
            var hourlyRate = employee?.HourlyRate ?? 300m;
            totalLaborCost += lb.HoursRequired * hourlyRate;
        }

        return totalMaterialCost + totalLaborCost;
    }

    private async Task AddDiningTableBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Dubová deska").Id, 1.62m, 5m, "Deska 180x90cm"),
            new MaterialBom(product.Id, GetMaterial("Bukové latě").Id, 6.0m, 3m, "Nohy stolu"),
            new MaterialBom(product.Id, GetMaterial("Nerezové šrouby").Id, 24.0m, 0m),
            new MaterialBom(product.Id, GetMaterial("Bezbarvý lak").Id, 0.5m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Hlavní truhlář", 6.0m, "Výroba desky a konstrukce", GetEmployee("Hlavní truhlář").Id, 1),
            new LaborBom(product.Id, "Lakýrník", 3.0m, "Broušení a lakování", GetEmployee("Lakýrník").Id, 2),
            new LaborBom(product.Id, "Montážník", 1.5m, "Finální montáž", GetEmployee("Montážník").Id, 3)
        });
    }

    private async Task AddKitchenCabinetBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Lakované MDF").Id, 1.8m, 5m),
            new MaterialBom(product.Id, GetMaterial("Překližka březová").Id, 0.5m, 3m),
            new MaterialBom(product.Id, GetMaterial("Kuličkové výsuvy").Id, 2.0m, 0m),
            new MaterialBom(product.Id, GetMaterial("Ložiskové panty").Id, 4.0m, 0m),
            new MaterialBom(product.Id, GetMaterial("Nábytkové knoflíky").Id, 2.0m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Truhlář", 4.0m, "Výroba korpusu a šuplíků", GetEmployee("Truhlář").Id, 1),
            new LaborBom(product.Id, "Montážník", 1.5m, "Montáž kování a finalizace", GetEmployee("Montážník").Id, 2)
        });
    }

    private async Task AddUpholsteredChairBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Bukové latě").Id, 2.5m, 5m, "Konstrukce židle"),
            new MaterialBom(product.Id, GetMaterial("Pěnová výplň").Id, 0.4m, 2m),
            new MaterialBom(product.Id, GetMaterial("Potahová látka").Id, 0.8m, 3m),
            new MaterialBom(product.Id, GetMaterial("Nerezové šrouby").Id, 12.0m, 0m),
            new MaterialBom(product.Id, GetMaterial("Lazura na dřevo").Id, 0.1m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Truhlář", 2.0m, "Výroba dřevěné konstrukce", GetEmployee("Truhlář").Id, 1),
            new LaborBom(product.Id, "Čalouník", 2.0m, "Čalounění sedáku a opěradla", GetEmployee("Čalouník").Id, 2),
            new LaborBom(product.Id, "Lakýrník", 1.0m, "Nátěr dřevěných částí", GetEmployee("Lakýrník").Id, 3)
        });
    }

    private async Task AddBookshelfBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Lakované MDF").Id, 4.5m, 5m, "Korpus a police"),
            new MaterialBom(product.Id, GetMaterial("Bezpečnostní sklo").Id, 0.8m, 2m, "Dvířka"),
            new MaterialBom(product.Id, GetMaterial("Ložiskové panty").Id, 4.0m, 0m),
            new MaterialBom(product.Id, GetMaterial("Nerezové šrouby").Id, 32.0m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Truhlář", 5.0m, "Výroba korpusu a polic", GetEmployee("Truhlář").Id, 1),
            new LaborBom(product.Id, "Montážník", 2.0m, "Montáž skla a kování", GetEmployee("Montážník").Id, 2)
        });
    }

    private async Task AddCoffeeTableBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Bukové latě").Id, 3.0m, 3m, "Konstrukce"),
            new MaterialBom(product.Id, GetMaterial("Zrcadlo").Id, 0.54m, 2m, "Deska 90x60cm"),
            new MaterialBom(product.Id, GetMaterial("Nerezové šrouby").Id, 16.0m, 0m),
            new MaterialBom(product.Id, GetMaterial("Bezbarvý lak").Id, 0.2m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Truhlář", 3.0m, "Výroba konstrukce", GetEmployee("Truhlář").Id, 1),
            new LaborBom(product.Id, "Lakýrník", 1.5m, "Broušení a lakování", GetEmployee("Lakýrník").Id, 2),
            new LaborBom(product.Id, "Montážník", 1.0m, "Montáž zrcadla", GetEmployee("Montážník").Id, 3)
        });
    }

    private async Task AddOfficeChairBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Bukové latě").Id, 1.5m, 3m, "Konstrukce"),
            new MaterialBom(product.Id, GetMaterial("Pěnová výplň").Id, 0.6m, 2m),
            new MaterialBom(product.Id, GetMaterial("Koženka hnědá").Id, 1.2m, 5m),
            new MaterialBom(product.Id, GetMaterial("Nerezové šrouby").Id, 20.0m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Truhlář", 2.5m, "Výroba dřevěné konstrukce", GetEmployee("Truhlář").Id, 1),
            new LaborBom(product.Id, "Čalouník", 2.5m, "Čalounění sedáku a opěradla", GetEmployee("Čalouník").Id, 2),
            new LaborBom(product.Id, "Montážník", 1.0m, "Montáž mechanismu", GetEmployee("Montážník").Id, 3)
        });
    }

    private async Task SeedOrdersAsync()
    {
        var products = await _context.Products.ToListAsync();
        if (!products.Any())
        {
            return;
        }

        var diningTable = products.First(p => p.Article == "ST-001");
        var kitchenCabinet = products.First(p => p.Article == "SK-002");
        var chair = products.First(p => p.Article == "ZI-003");
        var bookshelf = products.First(p => p.Article == "SK-004");
        var coffeeTable = products.First(p => p.Article == "ST-005");
        var officeChair = products.First(p => p.Article == "ZI-006");
        var tvStand = products.First(p => p.Article == "ST-007");

        var order1 = new Order(
            orderNumber: "2025-001",
            customerName: "Jan Novák",
            customerPhone: "+420 777 123 456",
            deliveryAddress: "Pražská 123, Praha 1, 110 00",
            expectedCompletionDate: DateTime.UtcNow.AddDays(14),
            customerEmail: "jan.novak@email.cz",
            notes: "Jídelní set - stůl + 4 židle"
        );
        await _context.Orders.AddAsync(order1);
        await _context.SaveChangesAsync();

        await _context.OrderItems.AddRangeAsync(new[]
        {
            new OrderItem(order1.Id, diningTable.Id, diningTable.Name, diningTable.Article, 1, diningTable.SalePrice),
            new OrderItem(order1.Id, chair.Id, chair.Name, chair.Article, 4, chair.SalePrice)
        });

        var order2 = new Order(
            orderNumber: "2025-002",
            customerName: "Marie Svobodová",
            customerPhone: "+420 602 987 654",
            deliveryAddress: "Brněnská 45, Brno, 602 00",
            expectedCompletionDate: DateTime.UtcNow.AddDays(9),
            customerEmail: "marie.svobodova@email.cz",
            notes: "Kancelář - TV stolek a knihovna"
        );
        await _context.Orders.AddAsync(order2);
        await _context.SaveChangesAsync();

        await _context.OrderItems.AddRangeAsync(new[]
        {
            new OrderItem(order2.Id, tvStand.Id, tvStand.Name, tvStand.Article, 1, tvStand.SalePrice),
            new OrderItem(order2.Id, bookshelf.Id, bookshelf.Name, bookshelf.Article, 2, bookshelf.SalePrice)
        });
        order2.ConfirmOrder();

        var order3 = new Order(
            orderNumber: "2025-003",
            customerName: "Petr Dvořák",
            customerPhone: "+420 723 456 789",
            deliveryAddress: "Ostravská 78, Ostrava, 702 00",
            expectedCompletionDate: DateTime.UtcNow.AddDays(-6),
            customerEmail: "petr.dvorak@email.cz",
            notes: "Kuchyňský nábytek"
        );
        await _context.Orders.AddAsync(order3);
        await _context.SaveChangesAsync();

        await _context.OrderItems.AddRangeAsync(new[]
        {
            new OrderItem(order3.Id, kitchenCabinet.Id, kitchenCabinet.Name, kitchenCabinet.Article, 3, kitchenCabinet.SalePrice),
            new OrderItem(order3.Id, coffeeTable.Id, coffeeTable.Name, coffeeTable.Article, 1, coffeeTable.SalePrice)
        });
        order3.ConfirmOrder();
        order3.CompleteOrder();

        var order4 = new Order(
            orderNumber: "2025-004",
            customerName: "Kateřina Málková",
            customerPhone: "+420 731 888 999",
            deliveryAddress: "Hradecká 56, Hradec Králové, 500 03",
            expectedCompletionDate: DateTime.UtcNow.AddDays(4),
            customerEmail: "katerina.malkova@email.cz",
            notes: "Kancelářský nábytek pro firmu"
        );
        await _context.Orders.AddAsync(order4);
        await _context.SaveChangesAsync();

        await _context.OrderItems.AddRangeAsync(new[]
        {
            new OrderItem(order4.Id, officeChair.Id, officeChair.Name, officeChair.Article, 6, officeChair.SalePrice),
            new OrderItem(order4.Id, bookshelf.Id, bookshelf.Name, bookshelf.Article, 1, bookshelf.SalePrice)
        });
        order4.ConfirmOrder();

        await _context.SaveChangesAsync();
    }

    private async Task AddTvStandBomAsync(Product product, List<Material> materials, List<Employee> employees)
    {
        Material GetMaterial(string name) => materials.First(m => m.Name.Contains(name));
        Employee GetEmployee(string position) => employees.First(e => e.Position.Contains(position));

        await _context.MaterialBoms.AddRangeAsync(new[]
        {
            new MaterialBom(product.Id, GetMaterial("Dubová deska").Id, 0.72m, 5m, "Vrchní deska 160x45cm"),
            new MaterialBom(product.Id, GetMaterial("Lakované MDF").Id, 1.5m, 8m, "Boční stěny a police"),
            new MaterialBom(product.Id, GetMaterial("Bukové latě").Id, 3.0m, 5m, "Konstrukce 2 šuplíků"),
            new MaterialBom(product.Id, GetMaterial("Bezpečnostní sklo").Id, 0.3m, 3m, "Skleněná polička"),
            new MaterialBom(product.Id, GetMaterial("Bezbarvý lak").Id, 0.8m, 0m, "Lakování MDF částí"),
            new MaterialBom(product.Id, GetMaterial("Bezbarvý lak").Id, 0.3m, 0m, "Ochrana dubové desky"),
            new MaterialBom(product.Id, GetMaterial("Nerezové šrouby").Id, 32.0m, 0m)
        });

        await _context.LaborBoms.AddRangeAsync(new[]
        {
            new LaborBom(product.Id, "Truhlář", 4.5m, "Výroba konstrukce stolku, řezání a sestavení", GetEmployee("Truhlář").Id, 1),
            new LaborBom(product.Id, "Lakýrník", 3.0m, "Broušení povrchů, nanášení bílého laku na MDF a bezbarvého na dub", GetEmployee("Lakýrník").Id, 2),
            new LaborBom(product.Id, "Montážník", 2.0m, "Instalace šuplíkových vodítek, montáž skleněné police", GetEmployee("Montážník").Id, 3)
        });
    }
}
