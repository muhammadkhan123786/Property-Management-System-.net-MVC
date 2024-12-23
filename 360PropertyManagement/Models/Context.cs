using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    public class Context:DbContext
    {
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<Addresses> addresses { get; set; }
        public DbSet<PropertySubCategories> propertysubcategory { get; set; }
        public DbSet<MyAreas> MyAreasAds { get; set; }
        public DbSet<SaveAd> savead { get; set; }
        public DbSet<MyAreasAdss> myareas { get; set; }
        public DbSet<LatestNews> latestnews { get; set; }
        public DbSet<SubmitedAds> submitedads { get; set; }
        public DbSet<AdAgents> agentsads { get; set; }
        public DbSet<PropertyAds> propertyads { get; set; }
        public DbSet<PropertyRooms> propertyrooms { get; set; }
        public DbSet<MsgSender> msgsender { get; set; }
        public DbSet<PropertySizeMeasured> propertySize { get; set; }
        public DbSet<TopCategory> toppropertycategory { get; set; }
        public DbSet<MsgReceiver> msgreceiver { get; set; }
        public DbSet<MsgDetails> msgdetails { get; set; }
        public DbSet<MsgConversation> msgconversation { get; set; }
        public DbSet<LicenseProduct> LicenseProduct { get; set; }
        public DbSet<ImageNot> imagenot { get; set; }
        public DbSet<LicenseFees> LicenseFee { get; set; }
        public DbSet<Cities> cities { get; set; }
        public DbSet<SendMessages> sendmessages { get; set; }
        public DbSet<SendEmails> emails { get; set; }
        public DbSet<Contacts> contacts { get; set; }
        public DbSet<RolesPermissions> rolepermissions { get; set; }
        public DbSet<AccountPermissions> accountpermissions { get; set; }
        public DbSet<Countries> countries { get; set; }
        public DbSet<Genders> genders { get; set; }
        public DbSet<Images> images { get; set; }
        public DbSet<Occupations> occupations { get; set; }
        public DbSet<Persons> persons { get; set; }
        public DbSet<Profiles> profiles { get; set; }
        public DbSet<Roles> roles { get; set; }
        public DbSet<States> states { get; set; }
        

    }
}