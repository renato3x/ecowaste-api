using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoWasteApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO recycling_centers (name, address, city, state, contact_phone, created_at)
SELECT 'Centro Verde Paulista', 'Avenida Paulista, 1000', 'Sao Paulo', 'SP', '1130001000', '2026-06-01 08:00:00'
WHERE NOT EXISTS (
    SELECT 1 FROM recycling_centers WHERE name = 'Centro Verde Paulista'
);");

            migrationBuilder.Sql(@"
INSERT INTO recycling_centers (name, address, city, state, contact_phone, created_at)
SELECT 'Recicla Norte', 'Rua das Arvores, 250', 'Sao Paulo', 'SP', '1130002000', '2026-06-01 08:05:00'
WHERE NOT EXISTS (
    SELECT 1 FROM recycling_centers WHERE name = 'Recicla Norte'
);");

            migrationBuilder.Sql(@"
INSERT INTO recycling_centers (name, address, city, state, contact_phone, created_at)
SELECT 'Eco Ponto Sul', 'Rua da Praia, 75', 'Santos', 'SP', '1330003000', '2026-06-01 08:10:00'
WHERE NOT EXISTS (
    SELECT 1 FROM recycling_centers WHERE name = 'Eco Ponto Sul'
);");

            migrationBuilder.Sql(@"
INSERT INTO waste_records (waste_type, weight_kg, collection_date, location, status, recycling_center_id, created_at)
SELECT
    'Plastic',
    45.50,
    '2026-06-01 09:00:00',
    'Zona Sul',
    'PendingCollection',
    (SELECT id FROM recycling_centers WHERE name = 'Centro Verde Paulista' LIMIT 1),
    '2026-06-01 09:05:00'
WHERE
    EXISTS (SELECT 1 FROM recycling_centers WHERE name = 'Centro Verde Paulista')
    AND NOT EXISTS (
        SELECT 1
        FROM waste_records
        WHERE waste_type = 'Plastic'
          AND location = 'Zona Sul'
          AND collection_date = '2026-06-01 09:00:00'
    );");

            migrationBuilder.Sql(@"
INSERT INTO waste_records (waste_type, weight_kg, collection_date, location, status, recycling_center_id, created_at)
SELECT
    'Glass',
    30.00,
    '2026-06-02 10:00:00',
    'Zona Norte',
    'Collected',
    (SELECT id FROM recycling_centers WHERE name = 'Recicla Norte' LIMIT 1),
    '2026-06-02 10:05:00'
WHERE
    EXISTS (SELECT 1 FROM recycling_centers WHERE name = 'Recicla Norte')
    AND NOT EXISTS (
        SELECT 1
        FROM waste_records
        WHERE waste_type = 'Glass'
          AND location = 'Zona Norte'
          AND collection_date = '2026-06-02 10:00:00'
    );");

            migrationBuilder.Sql(@"
INSERT INTO waste_records (waste_type, weight_kg, collection_date, location, status, recycling_center_id, created_at)
SELECT
    'Paper',
    52.25,
    '2026-06-03 11:00:00',
    'Zona Leste',
    'Processed',
    (SELECT id FROM recycling_centers WHERE name = 'Eco Ponto Sul' LIMIT 1),
    '2026-06-03 11:05:00'
WHERE
    EXISTS (SELECT 1 FROM recycling_centers WHERE name = 'Eco Ponto Sul')
    AND NOT EXISTS (
        SELECT 1
        FROM waste_records
        WHERE waste_type = 'Paper'
          AND location = 'Zona Leste'
          AND collection_date = '2026-06-03 11:00:00'
    );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM waste_records
WHERE (waste_type = 'Plastic' AND location = 'Zona Sul' AND collection_date = '2026-06-01 09:00:00')
   OR (waste_type = 'Glass' AND location = 'Zona Norte' AND collection_date = '2026-06-02 10:00:00')
   OR (waste_type = 'Paper' AND location = 'Zona Leste' AND collection_date = '2026-06-03 11:00:00');");

            migrationBuilder.Sql(@"
DELETE FROM recycling_centers rc
WHERE rc.name IN ('Centro Verde Paulista', 'Recicla Norte', 'Eco Ponto Sul')
  AND NOT EXISTS (
      SELECT 1
      FROM waste_records wr
      WHERE wr.recycling_center_id = rc.id
  );");
        }
    }
}
