CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `�d�߷s�W�f��` AS
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
        `goods`.`remark` AS `remark`,
        `goods`.`created_date` AS `created_date`,
        `goods`.`updated_date` AS `updated_date`
    FROM
        ((`goods`
        JOIN `brand`)
        JOIN `category`)
    WHERE
        ((`goods`.`categoryID` = `category`.`categoryID`)
            AND (`goods`.`brandID` = `brand`.`brandID`))
    ORDER BY `goods`.`goodsID`