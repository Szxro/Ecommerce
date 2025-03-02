﻿// <auto-generated />
using System;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecommerce.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250221185220_Template_Tables")]
    partial class Template_Tables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Domain.Entities.Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("DeletedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at_utc");

                    b.Property<string>("HashValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("hash_value");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.Property<string>("SaltValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("salt_value");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_credentials");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_credentials_user_id");

                    b.ToTable("credentials", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.EmailCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("code");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("DeletedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at_utc");

                    b.Property<DateTime>("ExpirationDateAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("expiration_date_at_utc");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsExpired")
                        .HasColumnType("bit")
                        .HasColumnName("is_expired");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit")
                        .HasColumnName("is_revoked");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit")
                        .HasColumnName("is_used");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_email_code");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_email_code_user_id");

                    b.ToTable("email_code", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("DeletedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at_utc");

                    b.Property<DateTime>("ExpirationDateAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("expiration_date_at_utc");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsExpired")
                        .HasColumnType("bit")
                        .HasColumnName("is_expired");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit")
                        .HasColumnName("is_revoked");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit")
                        .HasColumnName("is_used");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("token");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_refresh_token");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_token_user_id");

                    b.ToTable("refresh_token", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("DeletedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at_utc");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit")
                        .HasColumnName("is_default");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.Property<int>("TemplateCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("template_category_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_template");

                    b.HasIndex("TemplateCategoryId")
                        .HasDatabaseName("ix_template_template_category_id");

                    b.ToTable("template", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.TemplateCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("DeletedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at_utc");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_template_category");

                    b.ToTable("template_category", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int")
                        .HasColumnName("access_failed_count");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("DeletedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at_utc");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit")
                        .HasColumnName("is_admin");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit")
                        .HasColumnName("is_email_verified");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("last_name");

                    b.Property<bool>("LockOutEnabled")
                        .HasColumnType("bit")
                        .HasColumnName("lock_out_enabled");

                    b.Property<DateTime>("LockOutEndAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("lock_out_end_at_utc");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.UserTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at_utc");

                    b.Property<DateTime>("ModifiedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at_utc");

                    b.Property<int>("TemplateId")
                        .HasColumnType("int")
                        .HasColumnName("template_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_template");

                    b.HasIndex("TemplateId")
                        .HasDatabaseName("ix_user_template_template_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_template_user_id");

                    b.ToTable("user_template", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.Credentials", b =>
                {
                    b.HasOne("Ecommerce.Domain.Entities.User", "User")
                        .WithMany("Credentials")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_credentials_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.EmailCode", b =>
                {
                    b.HasOne("Ecommerce.Domain.Entities.User", "User")
                        .WithMany("EmailCodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_email_code_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("Ecommerce.Domain.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_token_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.Template", b =>
                {
                    b.HasOne("Ecommerce.Domain.Entities.TemplateCategory", "TemplateCategory")
                        .WithMany("Templates")
                        .HasForeignKey("TemplateCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_template_template_category_template_category_id");

                    b.Navigation("TemplateCategory");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.User", b =>
                {
                    b.OwnsOne("Ecommerce.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(450)")
                                .HasColumnName("email");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("ix_user_email_value");

                            b1.ToTable("user");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_user_user_id");
                        });

                    b.OwnsOne("Ecommerce.Domain.ValueObjects.Username", "Username", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(450)")
                                .HasColumnName("username");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("ix_user_username_value");

                            b1.ToTable("user");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_user_user_id");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Username")
                        .IsRequired();
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.UserTemplate", b =>
                {
                    b.HasOne("Ecommerce.Domain.Entities.Template", "Template")
                        .WithMany("Templates")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_template_template_template_id");

                    b.HasOne("Ecommerce.Domain.Entities.User", "User")
                        .WithMany("Templates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_template_user_user_id");

                    b.Navigation("Template");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.Template", b =>
                {
                    b.Navigation("Templates");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.TemplateCategory", b =>
                {
                    b.Navigation("Templates");
                });

            modelBuilder.Entity("Ecommerce.Domain.Entities.User", b =>
                {
                    b.Navigation("Credentials");

                    b.Navigation("EmailCodes");

                    b.Navigation("RefreshTokens");

                    b.Navigation("Templates");
                });
#pragma warning restore 612, 618
        }
    }
}
