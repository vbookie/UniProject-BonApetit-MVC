namespace Cooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDateForRecipe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "CreateDate");
        }
    }
}
