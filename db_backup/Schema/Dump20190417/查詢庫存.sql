CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `查詢庫存` AS
    SELECT 
        `goods`.`goodsID` AS `goodsID`,
        `goods`.`type` AS `type`,
        `category`.`categoryName` AS `categoryName`,
        `category`.`engAlias` AS `engAlias`,
        `goods`.`spec` AS `spec`,
        `brand`.`brandName` AS `brandName`,
        CONCAT(UPPER(`category`.`engAlias`),
                '-',
                `goods`.`goodsID`) AS `barcode`,
        `goods`.`quantity` AS `quantity`,
        `goods`.`remark` AS `remark`
    FROM
        ((`goods`
        JOIN `brand`)
        JOIN `category`)
    WHERE
        ((`goods`.`categoryID` = `category`.`categoryID`)
            AND (`goods`.`brandID` = `brand`.`brandID`))
    ORDER BY `goods`.`goodsID`