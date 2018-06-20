namespace WCFServer.Data.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<sys_dictionary_mst> sys_dictionary_mst { get; set; }
        public virtual DbSet<sys_group_mst> sys_group_mst { get; set; }
        public virtual DbSet<sys_language_mst> sys_language_mst { get; set; }
        public virtual DbSet<sys_server_info_mst> sys_server_info_mst { get; set; }
        public virtual DbSet<sys_user_mst> sys_user_mst { get; set; }
        public virtual DbSet<sys_log_mst> sys_log_mst { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.thread)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.level)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.logger)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.message)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.operand)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.machine_name)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.browser)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log_mst>()
                .Property(e => e.exception)
                .IsUnicode(false);
        }
    }
}
