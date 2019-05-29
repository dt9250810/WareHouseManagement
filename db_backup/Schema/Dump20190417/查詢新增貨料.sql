CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `ct_warehouse_db`.`查詢新增貨料` AS
    SELECT 
        `ct_warehouse_db`.`goods`.`goodsID` AS `goodsID`,
        `ct_warehouse_db`.`goods`.`type` AS `type`,
        `ct_warehouse_db`.`category`.`categoryName` AS `categoryName`,
        `ct_warehouse_db`.`category`.`engAlias` AS `engAlias`,
        `ct_warehouse_db`.`goods`.`spec` AS `spec`,
        `ct_warehouse_db`.`brand`.`brandName` AS `brandName`,
        CONCAT(UPPER(`ct_warehouse_db`.`category`.`engAlias`),
                '-',
                `ct_warehouse_db`.`goods`.`goodsID`) AS `barcode`,
        `ct_warehouse_db`.`goods`.`quantity` AS `quantity`,
        `ct_warehouse_db`.`goods`.`remark` AS `remark`,
        `ct_warehouse_db`.`goods`.`created_date` AS `created_date`,
        `ct_warehouse_db`.`goods`.`updated_date` AS `updated_date`
    FROM
        ((`ct_warehouse_db`.`goods`
        JOIN `ct_warehouse_db`.`brand`)
        JOIN `ct_warehouse_db`.`category`)
    WHERE
        ((`ct_warehouse_db`.`goods`.`categoryID` = `ct_warehouse_db`.`category`.`categoryID`)
            AND (`ct_warehouse_db`.`goods`.`brandID` = `ct_warehouse_db`.`brand`.`brandID`))
    ORDER BY `ct_warehouse_db`.`goods`.`goodsID`
