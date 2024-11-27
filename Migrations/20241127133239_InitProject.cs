using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5PJS.Migrations
{
    /// <inheritdoc />
    public partial class InitProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Complement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.Id);
                });

            migrationBuilder.CreateTable(
               name: "User",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   DateBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_users", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_administrator_user_IdUser", // Nome da chave estrangeira
                        column: x => x.IdUser,       // Coluna na tabela "administrators"
                        principalTable: "User",    // Tabela referenciada
                        principalColumn: "Id",      // Coluna na tabela "users"
                        onDelete: ReferentialAction.Cascade // Comportamento ao excluir
                    );
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Crm = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_doctor_User_IdUser",
                        column: x => x.IdUser,        // Campo na tabela "diagnosiss"
                        principalTable: "User",       // Tabela referenciada
                        principalColumn: "Id",         // Campo único na tabela "Doctor"
                        onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                    );
                });
           
            migrationBuilder.CreateIndex(
               name: "IX_Doctor_Crm",
               table: "Doctor",
               column: "Crm",
               unique: true); // Torna Crm único

            migrationBuilder.CreateTable(
               name: "Patient",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   CrmDoctor = table.Column<string>(type: "nvarchar(20)", nullable: false),
                   Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   DateBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                   Cpf = table.Column<string>(type: "nvarchar(11)", nullable: false),
                   IdAddress = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_patient", x => x.Id);
                   table.ForeignKey(
                       name: "FK_patient_Doctor_CrmDoctor",
                       column: x => x.CrmDoctor,        // Campo na tabela "diagnosiss"
                       principalTable: "Doctor",       // Tabela referenciada
                       principalColumn: "Crm",         // Campo único na tabela "Doctor"
                       onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                   );
                   table.ForeignKey(
                       name: "FK_patient_Address_IdAddress",
                       column: x => x.IdAddress,        // Campo na tabela "diagnosiss"
                       principalTable: "Address",       // Tabela referenciada
                       principalColumn: "Id",         // Campo único na tabela "Doctor"
                       onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                   );

               });
            migrationBuilder.CreateIndex(
              name: "IX_Patient_Cpf",
              table: "Patient",
              column: "Cpf",
              unique: true); // Torna Crm único
            migrationBuilder.CreateTable(
                name: "MethodDiagnosis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrmDoctor = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Effectiveness = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    ResponseTime = table.Column<int>(type: "int", nullable: false),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_methodDiagnosis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_methodDiagnosis_Doctor_CrmDoctor",
                        column: x => x.CrmDoctor,        // Campo na tabela "diagnosiss"
                        principalTable: "Doctor",       // Tabela referenciada
                        principalColumn: "Crm",         // Campo único na tabela "Doctor"
                        onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                    );
                });

            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrmDoctor = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CpfPatient = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<bool>(type: "bit", nullable: false),
                    IdMethodDiagnosis = table.Column<int>(type: "int", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diagnosis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_diagnosis_Doctor_CrmDoctor",
                        column: x => x.CrmDoctor,        // Campo na tabela "diagnosiss"
                        principalTable: "Doctor",       // Tabela referenciada
                        principalColumn: "Crm",         // Campo único na tabela "Doctor"
                        onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                    );
                    table.ForeignKey(
                        name: "FK_diagnosis_Patient_CpfPatient",
                        column: x => x.CpfPatient,        // Campo na tabela "diagnosiss"
                        principalTable: "Patient",       // Tabela referenciada
                        principalColumn: "Cpf",         // Campo único na tabela "Doctor"
                        onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                    );
                    table.ForeignKey(
                        name: "FK_diagnosis_MethodDiagnosis_IdMethodDiagnosis",
                        column: x => x.IdMethodDiagnosis,        // Campo na tabela "diagnosiss"
                        principalTable: "MethodDiagnosis",       // Tabela referenciada
                        principalColumn: "Id",         // Campo único na tabela "Doctor"
                        onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                    );
                });

            

           

            

           
         

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CrmDoctor = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.Id);
                    table.ForeignKey(
                       name: "FK_report_Doctor_CrmDoctor",
                       column: x => x.CrmDoctor,        // Campo na tabela "diagnosiss"
                       principalTable: "Doctor",       // Tabela referenciada
                       principalColumn: "Crm",         // Campo único na tabela "Doctor"
                       onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                   );
                });

           

            migrationBuilder.CreateTable(
                name: "FeedBack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrmDoctor = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    IdDiagnosis = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestDate = table.Column<int>(type: "int", nullable: false),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedBack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedBack_Diagnosis_IdDiagnosis",
                        column: x => x.IdDiagnosis,
                        principalTable: "Diagnosis",
                        principalColumn: "Id");
                    table.ForeignKey(
                       name: "FK_feedBack_Doctor_CrmDoctor",
                       column: x => x.CrmDoctor,        // Campo na tabela "diagnosiss"
                       principalTable: "Doctor",       // Tabela referenciada
                       principalColumn: "Crm",         // Campo único na tabela "Doctor"
                       onDelete: ReferentialAction.Restrict // Escolha o comportamento desejado
                   );
                });



            migrationBuilder.CreateTable(
                name: "ReportDiagnosis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReport = table.Column<int>(type: "int", nullable: false),
                    IdDiagnosis = table.Column<int>(type: "int", nullable: false),
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportDiagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reportDiagnoses_diagnosiss_IdDiagnosis",
                        column: x => x.IdDiagnosis,
                        principalTable: "Diagnosis",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_reportDiagnosis_Report_IdReport",
                        column: x => x.IdReport,
                        principalTable: "Report",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
               name: "IX_administrator_IdUser",
               table: "Administrator",
               column: "IdUser"
               );
            migrationBuilder.CreateIndex(
               name: "IX_diagnosis_IdMethodDiagnosis",
               table: "Diagnosis",
               column: "IdMethodDiagnosis"
               );
            migrationBuilder.CreateIndex(
               name: "IX_diagnosis_CpfPatient",
               table: "Diagnosis",
               column: "CpfPatient"
               );
            migrationBuilder.CreateIndex(
               name: "IX_diagnosis_CrmDoctor",
               table: "Diagnosis",
               column: "CrmDoctor"
               );
            migrationBuilder.CreateIndex(
               name: "IX_doctor_IdUser",
               table: "Doctor",
               column: "IdUser"
               );
            migrationBuilder.CreateIndex(
               name: "IX_methodDiagnosis_CrmDoctor",
               table: "MethodDiagnosis",
               column: "CrmDoctor"
               );
            migrationBuilder.CreateIndex(
               name: "IX_patient_IdAddress",
               table: "Patient",
               column: "IdAddress"
               );
            migrationBuilder.CreateIndex(
               name: "IX_patient_CrmDoctor",
               table: "Patient",
               column: "CrmDoctor"
               );
            migrationBuilder.CreateIndex(
               name: "IX_report_CrmDoctor",
               table: "Report",
               column: "CrmDoctor"
               );
            migrationBuilder.CreateIndex(
                name: "IX_feedBack_CrmDoctor",
                table: "FeedBack",
                column: "CrmDoctor"
                );
            migrationBuilder.CreateIndex(
                name: "IX_feedBack_IdDiagnosis",
                table: "FeedBack",
                column: "IdDiagnosis");

            migrationBuilder.CreateIndex(
                name: "IX_reportDiagnosis_IdDiagnosis",
                table: "ReportDiagnosis",
                column: "IdDiagnosis");

            migrationBuilder.CreateIndex(
                name: "IX_reportDiagnosis_IdReport",
                table: "ReportDiagnosis",
                column: "IdReport");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "FeedBack");

            migrationBuilder.DropTable(
                name: "MethodDiagnosis");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "ReportDiagnosis");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "Report");
        }
    }
}
