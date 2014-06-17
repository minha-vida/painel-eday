namespace EdeaDay.GestaoDeIdeias.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoverAprovacao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ideias", "Aprovado");
            DropColumn("dbo.Ideias", "DataAprovacao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ideias", "DataAprovacao", c => c.DateTime());
            AddColumn("dbo.Ideias", "Aprovado", c => c.Boolean(nullable: false));
        }
    }
}
