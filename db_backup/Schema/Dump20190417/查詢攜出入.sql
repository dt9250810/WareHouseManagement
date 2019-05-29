CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `ct_warehouse_db`.`查詢攜出入` AS
    SELECT 
        (CASE `ct_warehouse_db`.`bring`.`type`
            WHEN '0' THEN '攜出'
            WHEN '1' THEN '攜回'
        END) AS `bring_type`,
        `ct_warehouse_db`.`goods`.`type` AS `type`,
        `ct_warehouse_db`.`category`.`categoryName` AS `categoryName`,
        `ct_warehouse_db`.`brand`.`brandName` AS `brandName`,
        `ct_warehouse_db`.`bring`.`quantity` AS `quantity`,
        `ct_warehouse_db`.`bring`.`created_date` AS `created_date`,
        `ct_warehouse_db`.`staff`.`name` AS `name`
    FROM
        ((((`ct_warehouse_db`.`bring`
        JOIN `ct_warehouse_db`.`goods`)
        JOIN `ct_warehouse_db`.`staff`)
        JOIN `ct_warehouse_db`.`category`)
        JOIN `ct_warehouse_db`.`brand`)
    WHERE
        ((`ct_warehouse_db`.`bring`.`goodsID` = `ct_warehouse_db`.`goods`.`goodsID`)
            AND (`ct_warehouse_db`.`bring`.`staffID` = `ct_warehouse_db`.`staff`.`id`)
            AND (`ct_warehouse_db`.`goods`.`brandID` = `ct_warehouse_db`.`brand`.`brandID`)
            AND (`ct_warehouse_db`.`goods`.`categoryID` = `ct_warehouse_db`.`category`.`categoryID`))
    ORDER BY `ct_warehouse_db`.`bring`.`created_date` DESC
