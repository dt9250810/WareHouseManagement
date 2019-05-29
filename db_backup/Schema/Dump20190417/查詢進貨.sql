CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `ct_warehouse_db`.`查詢進貨` AS
    SELECT 
        `ct_warehouse_db`.`purchase`.`goodsID` AS `goodsID`,
        `ct_warehouse_db`.`goods`.`type` AS `type`,
        `ct_warehouse_db`.`category`.`categoryName` AS `categoryName`,
        `ct_warehouse_db`.`category`.`engAlias` AS `engAlias`,
        `ct_warehouse_db`.`goods`.`spec` AS `spec`,
        `ct_warehouse_db`.`brand`.`brandName` AS `brandName`,
        CONCAT(UPPER(`ct_warehouse_db`.`category`.`engAlias`),
                '-',
                `ct_warehouse_db`.`goods`.`goodsID`) AS `barcode`,
        `ct_warehouse_db`.`purchase`.`quantity` AS `quantity`,
        `ct_warehouse_db`.`purchase`.`created_date` AS `created_date`
    FROM
        (((`ct_warehouse_db`.`purchase`
        JOIN `ct_warehouse_db`.`goods`)
        JOIN `ct_warehouse_db`.`category`)
        JOIN `ct_warehouse_db`.`brand`)
    WHERE
        ((`ct_warehouse_db`.`purchase`.`goodsID` = `ct_warehouse_db`.`goods`.`goodsID`)
            AND (`ct_warehouse_db`.`goods`.`categoryID` = `ct_warehouse_db`.`category`.`categoryID`)
            AND (`ct_warehouse_db`.`goods`.`brandID` = `ct_warehouse_db`.`brand`.`brandID`))
    ORDER BY `ct_warehouse_db`.`purchase`.`created_date` DESC , `ct_warehouse_db`.`purchase`.`quantity` ASC
