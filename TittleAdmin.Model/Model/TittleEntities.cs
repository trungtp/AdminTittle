namespace TittleAdmin.Model.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Data.Common;

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class TittleEntities : DbContext
    {
        public TittleEntities()
            : base(nameOrConnectionString: "TittleEntities")
        {
        }
        
        public virtual DbSet<access> accesses { get; set; }
        public virtual DbSet<action> actions { get; set; }
        public virtual DbSet<activity> activities { get; set; }
        public virtual DbSet<add_ons> add_ons { get; set; }
        public virtual DbSet<app_usage> app_usage { get; set; }
        public virtual DbSet<application> applications { get; set; }
        public virtual DbSet<command> commands { get; set; }
        public virtual DbSet<commands_android> commands_android { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<config> configs { get; set; }
        public virtual DbSet<data_limit> data_limit { get; set; }
        public virtual DbSet<device_installations> device_installations { get; set; }
        public virtual DbSet<device> devices { get; set; }
        public virtual DbSet<geofencing> geofencings { get; set; }
        public virtual DbSet<job> jobs { get; set; }
        public virtual DbSet<key_sandboxs> key_sandboxs { get; set; }
        public virtual DbSet<key> keys { get; set; }
        public virtual DbSet<kid_package> kid_package { get; set; }
        public virtual DbSet<kid> kids { get; set; }
        public virtual DbSet<language> languages { get; set; }
        public virtual DbSet<link_block> link_block { get; set; }
        public virtual DbSet<link_usage> link_usage { get; set; }
        public virtual DbSet<location_landmark> location_landmark { get; set; }
        public virtual DbSet<location> locations { get; set; }
        public virtual DbSet<map> maps { get; set; }
        public virtual DbSet<notification_boxes> notification_boxes { get; set; }
        public virtual DbSet<notification_messages> notification_messages { get; set; }
        public virtual DbSet<notification> notifications { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<package> packages { get; set; }
        public virtual DbSet<payment_histories> payment_histories { get; set; }
        public virtual DbSet<perks_points> perks_points { get; set; }
        public virtual DbSet<perks_tasks> perks_tasks { get; set; }
        public virtual DbSet<plan> plans { get; set; }
        public virtual DbSet<promo_codes> promo_codes { get; set; }
        public virtual DbSet<redeem> redeems { get; set; }
        public virtual DbSet<reminder> reminders { get; set; }
        public virtual DbSet<schedule> schedules { get; set; }
        public virtual DbSet<schedulesv2> schedulesv2 { get; set; }
        public virtual DbSet<send_mail> send_mail { get; set; }
        public virtual DbSet<setting> settings { get; set; }
        public virtual DbSet<slot> slots { get; set; }
        public virtual DbSet<system_settings> system_settings { get; set; }
        public virtual DbSet<task_schedules> task_schedules { get; set; }
        public virtual DbSet<task_schedules_trigger> task_schedules_trigger { get; set; }
        public virtual DbSet<translation> translations { get; set; }
        public virtual DbSet<translations_sandbox> translations_sandbox { get; set; }
        public virtual DbSet<user_package> user_package { get; set; }
        public virtual DbSet<user_plans> user_plans { get; set; }
        public virtual DbSet<user_promo_code> user_promo_code { get; set; }
        public virtual DbSet<user_redeem> user_redeem { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<zone> zones { get; set; }
        public virtual DbSet<migration> migrations { get; set; }
        public virtual DbSet<password_resets> password_resets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<access>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<access>()
                .Property(e => e.package)
                .IsUnicode(false);

            modelBuilder.Entity<action>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<action>()
                .Property(e => e.longtitude)
                .IsUnicode(false);

            modelBuilder.Entity<action>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<action>()
                .Property(e => e.action1)
                .IsUnicode(false);

            modelBuilder.Entity<action>()
                .Property(e => e.app_name)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.longtitude)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<add_ons>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<app_usage>()
                .Property(e => e.app_name)
                .IsUnicode(false);

            modelBuilder.Entity<application>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<application>()
                .Property(e => e.package)
                .IsUnicode(false);

            modelBuilder.Entity<command>()
                .Property(e => e.udid)
                .IsUnicode(false);

            modelBuilder.Entity<command>()
                .Property(e => e.command1)
                .IsUnicode(false);

            modelBuilder.Entity<command>()
                .Property(e => e._params)
                .IsUnicode(false);

            modelBuilder.Entity<command>()
                .Property(e => e.extra)
                .IsUnicode(false);

            modelBuilder.Entity<commands_android>()
                .Property(e => e.package)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.comment1)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<config>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<config>()
                .Property(e => e.package)
                .IsUnicode(false);

            modelBuilder.Entity<data_limit>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<data_limit>()
                .Property(e => e.unit)
                .IsUnicode(false);

            modelBuilder.Entity<device_installations>()
                .Property(e => e.unique_id)
                .IsUnicode(false);

            modelBuilder.Entity<device_installations>()
                .Property(e => e.os)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.deviceable_type)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.device_type)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.os)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.token)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.mobile_token)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.imei)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.model_number)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.manufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.udid)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.mdm)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.restrictions)
                .IsUnicode(false);

            modelBuilder.Entity<device>()
                .Property(e => e.language)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.landmark)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.repeat)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.indicate)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.period_type)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.timezone)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<geofencing>()
                .Property(e => e.kid_ids)
                .IsUnicode(false);

            modelBuilder.Entity<job>()
                .Property(e => e.queue)
                .IsUnicode(false);

            modelBuilder.Entity<job>()
                .Property(e => e.payload)
                .IsUnicode(false);

            modelBuilder.Entity<key_sandboxs>()
                .Property(e => e.key)
                .IsUnicode(false);

            modelBuilder.Entity<key_sandboxs>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<key>()
                .Property(e => e.key1)
                .IsUnicode(false);

            modelBuilder.Entity<key>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.kid_token)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.timezone)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .Property(e => e.phone_version)
                .IsUnicode(false);

            modelBuilder.Entity<kid>()
                .HasMany(e => e.reminders)
                .WithRequired(e => e.kid)
                .HasForeignKey(e => e.kid_id);

            modelBuilder.Entity<language>()
                .Property(e => e.locale)
                .IsUnicode(false);

            modelBuilder.Entity<language>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<link_block>()
                .Property(e => e.kid_ids)
                .IsUnicode(false);

            modelBuilder.Entity<link_block>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<link_usage>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<link_usage>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<location_landmark>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<location>()
                .Property(e => e.longtitude)
                .IsUnicode(false);

            modelBuilder.Entity<map>()
                .Property(e => e.kid_name)
                .IsUnicode(false);

            modelBuilder.Entity<map>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<map>()
                .Property(e => e.longtitude)
                .IsUnicode(false);

            modelBuilder.Entity<map>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<map>()
                .Property(e => e.device_ids)
                .IsUnicode(false);

            modelBuilder.Entity<notification_boxes>()
                .Property(e => e.kid_token)
                .IsUnicode(false);

            modelBuilder.Entity<notification_boxes>()
                .Property(e => e.device_type)
                .IsUnicode(false);

            modelBuilder.Entity<notification_boxes>()
                .Property(e => e.message)
                .IsUnicode(false);

            modelBuilder.Entity<notification_boxes>()
                .Property(e => e.unread)
                .IsUnicode(false);

            modelBuilder.Entity<notification_messages>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.data)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<order>()
                .Property(e => e.currency)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.duration_unit)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.currency)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.appstore_id)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .Property(e => e.google_play_id)
                .IsUnicode(false);

            modelBuilder.Entity<payment_histories>()
                .Property(e => e.currency)
                .IsUnicode(false);

            modelBuilder.Entity<payment_histories>()
                .Property(e => e.payment_details)
                .IsUnicode(false);

            modelBuilder.Entity<perks_points>()
                .Property(e => e.friend_ids)
                .IsUnicode(false);

            modelBuilder.Entity<perks_tasks>()
                .Property(e => e.key)
                .IsUnicode(false);

            modelBuilder.Entity<perks_tasks>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<perks_tasks>()
                .Property(e => e.icon)
                .IsUnicode(false);

            modelBuilder.Entity<plan>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<plan>()
                .Property(e => e.multiple_kids)
                .IsUnicode(false);

            modelBuilder.Entity<plan>()
                .Property(e => e.multiple_login)
                .IsUnicode(false);

            modelBuilder.Entity<plan>()
                .Property(e => e.amount)
                .IsUnicode(false);

            modelBuilder.Entity<promo_codes>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<promo_codes>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<promo_codes>()
                .Property(e => e.rule)
                .IsUnicode(false);

            modelBuilder.Entity<promo_codes>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<promo_codes>()
                .Property(e => e.promo_type)
                .IsUnicode(false);

            modelBuilder.Entity<redeem>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<redeem>()
                .Property(e => e.frequency)
                .IsUnicode(false);

            modelBuilder.Entity<reminder>()
                .Property(e => e.task_name)
                .IsUnicode(false);

            modelBuilder.Entity<reminder>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<reminder>()
                .Property(e => e.repeat)
                .IsUnicode(false);

            modelBuilder.Entity<reminder>()
                .Property(e => e.group_id)
                .IsUnicode(false);

            modelBuilder.Entity<reminder>()
                .Property(e => e.timezone)
                .IsUnicode(false);

            modelBuilder.Entity<reminder>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.reminder)
                .HasForeignKey(e => e.reminder_id);

            modelBuilder.Entity<schedule>()
                .Property(e => e.repeat)
                .IsUnicode(false);

            modelBuilder.Entity<schedulesv2>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<schedulesv2>()
                .Property(e => e.options)
                .IsUnicode(false);

            modelBuilder.Entity<schedulesv2>()
                .Property(e => e.extra)
                .IsUnicode(false);

            modelBuilder.Entity<schedulesv2>()
                .Property(e => e.time_zone)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.user_id)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.kid_ids)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.subject)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.path)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<send_mail>()
                .Property(e => e.type_usage)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.entity_type)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<slot>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<system_settings>()
                .Property(e => e.key)
                .IsUnicode(false);

            modelBuilder.Entity<system_settings>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<system_settings>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<task_schedules>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<task_schedules>()
                .Property(e => e.repeat)
                .IsUnicode(false);

            modelBuilder.Entity<task_schedules>()
                .Property(e => e.options)
                .IsUnicode(false);

            modelBuilder.Entity<task_schedules>()
                .Property(e => e.extra)
                .IsUnicode(false);

            modelBuilder.Entity<task_schedules>()
                .Property(e => e.period_type)
                .IsUnicode(false);

            modelBuilder.Entity<task_schedules_trigger>()
                .Property(e => e.task_schedules_id)
                .IsUnicode(false);

            modelBuilder.Entity<translation>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<translations_sandbox>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<user_package>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.remember_token)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.activation_code)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.auth_token)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.multiple_login)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.multiple_kids)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.account_type)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.timezone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.id_facebook)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.country)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.reminders)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.parent_id);

            modelBuilder.Entity<zone>()
                .Property(e => e.country_code)
                .IsUnicode(false);

            modelBuilder.Entity<zone>()
                .Property(e => e.zone_name)
                .IsUnicode(false);

            modelBuilder.Entity<migration>()
                .Property(e => e.migration1)
                .IsUnicode(false);

            modelBuilder.Entity<password_resets>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<password_resets>()
                .Property(e => e.token)
                .IsUnicode(false);
        }
    }
}
