﻿using OpenOrm.SqlProvider.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//using MySql.Data;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using OpenOrm.Extensions;
using MySqlConnector;

namespace OpenOrm.SqlProvider.MySql
{
    class MySqlConnector : BaseConnector, ISqlConnector, IDisposable
    {
        #region Parameter
        //public void AddParameter(string paramName, object value)
        //{
        //    SqlDbType type = MySqlTools.DetectParameterType(value);
        //    AddParameter(paramName, value, type);
        //}

        public void AddParameter(string paramName, object value, SqlDbType type)
        {
            MySqlParameter param = new MySqlParameter();
            if (!paramName.StartsWith("@"))
            {
                paramName = "@" + paramName;
            }
            param.Direction = ParameterDirection.Input;
            param.ParameterName = paramName;
            param.DbType = type.ToDbType();
            param.Value = value;
            param.Size = GetParamSize(value);
            Parameters.Add(param);
        }

        public void ClearParameters()
        {
            Parameters = null;
            Parameters = new ArrayList();
        }
        #endregion

        #region Execute
        public long ExecuteStoredProcedure(OpenOrmDbConnection cnx, string storedprocedure)
        {
            long returnValue = 0;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)cnx.Connection;
            cmd.CommandText = storedprocedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = cnx.Transaction != null ? (MySqlTransaction)cnx.Transaction : null;

            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    Direction = ParameterDirection.ReturnValue,
            //    DbType = DbType.Int64,
            //    Scale = 0,
            //    Precision = 10,
            //    ParameterName = "@RETURN_VALUE"
            //});

            if (Parameters.Count > 0)
            {
                for (int i = 0; i <= Parameters.Count - 1; i++)
                {
                    cmd.Parameters.Add((MySqlParameter)Parameters[i]);
                }

                cmd.Prepare();
            }


            //cmd.Prepare();
            cmd.ExecuteNonQuery();

            //set values of output parameters
            if ((Parameters != null))
            {
                for (int i = 1; i <= cmd.Parameters.Count - 1; i++)
                {
                    if (cmd.Parameters[i].Direction != ParameterDirection.Input)
                    {
                        ((MySqlParameter)Parameters[i]).Value = cmd.Parameters[i].Value;
                        if (cmd.Parameters[i].ParameterName == "@RETURN_VALUE")
                            returnValue = (long)cmd.Parameters[i].Value;
                    }
                }
            }

            cmd.Dispose();

            return returnValue;
        }

        public async Task<long> ExecuteStoredProcedureAsync(OpenOrmDbConnection cnx, string storedprocedure)
        {
            long returnValue = 0;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)cnx.Connection;
            cmd.CommandText = storedprocedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = cnx.Transaction != null ? (MySqlTransaction)cnx.Transaction : null;

            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    Direction = ParameterDirection.ReturnValue,
            //    DbType = DbType.Int64,
            //    Scale = 0,
            //    Precision = 10,
            //    ParameterName = "@RETURN_VALUE"
            //});

            if (Parameters.Count > 0)
            {
                for (int i = 0; i <= Parameters.Count - 1; i++)
                {
                    cmd.Parameters.Add((MySqlParameter)Parameters[i]);
                }

                cmd.Prepare();
            }

            //string rawQuery = GetRawQuery(cmd);

            //cmd.Prepare();
            await cmd.ExecuteNonQueryAsync();

            //set values of output parameters
            if ((Parameters != null))
            {
                for (int i = 1; i <= cmd.Parameters.Count - 1; i++)
                {
                    if (cmd.Parameters[i].Direction != ParameterDirection.Input)
                    {
                        ((MySqlParameter)Parameters[i]).Value = cmd.Parameters[i].Value;
                        if (cmd.Parameters[i].ParameterName == "@RETURN_VALUE")
                            returnValue = (long)cmd.Parameters[i].Value;
                    }
                }
            }

            cmd.Dispose();

            return returnValue;
        }



        //Retourne la requete SQL complète (pour debug)
        static string GetRawQuery(MySqlCommand command)
        {
            var sb = new StringBuilder(command.CommandText);
            foreach (MySqlParameter p in command.Parameters)
            {
                sb = sb.Replace(p.ParameterName, "'" + p.Value.ToString() + "'");
            }
            return sb.ToString();
        }




        public long ExecuteSql(OpenOrmDbConnection cnx, string sql)
        {
            long returnValue = 0;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)cnx.Connection;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Transaction = cnx.Transaction != null ? (MySqlTransaction)cnx.Transaction : null;

            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    Direction = ParameterDirection.ReturnValue,
            //    DbType = DbType.Int64,
            //    Scale = 0,
            //    Precision = 10,
            //    ParameterName = "@RETURN_VALUE"
            //});

            if (Parameters.Count > 0)
            {
                for (int i = 0; i <= Parameters.Count - 1; i++)
                {
                    cmd.Parameters.Add((MySqlParameter)Parameters[i]);
                }

                cmd.Prepare();
            }


            //cmd.Prepare();
            cmd.ExecuteNonQuery();

            //set values of output parameters
            if ((Parameters != null))
            {
                for (int i = 1; i <= cmd.Parameters.Count - 1; i++)
                {
                    if (cmd.Parameters[i].Direction != ParameterDirection.Input)
                    {
                        ((MySqlParameter)Parameters[i]).Value = cmd.Parameters[i].Value;
                        if (cmd.Parameters[i].ParameterName == "@RETURN_VALUE")
                            returnValue = (long)cmd.Parameters[i].Value;
                    }
                }
            }

            cmd.Dispose();

            return returnValue;
        }

        public async Task<long> ExecuteSqlAsync(OpenOrmDbConnection cnx, string sql)
        {
            long returnValue = 0;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)cnx.Connection;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Transaction = cnx.Transaction != null ? (MySqlTransaction)cnx.Transaction : null;

            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    Direction = ParameterDirection.ReturnValue,
            //    DbType = DbType.Int64,
            //    Scale = 0,
            //    Precision = 10,
            //    ParameterName = "@RETURN_VALUE"
            //});

            if (Parameters.Count > 0)
            {
                for (int i = 0; i <= Parameters.Count - 1; i++)
                {
                    cmd.Parameters.Add((MySqlParameter)Parameters[i]);
                }

                cmd.Prepare();
            }


            //cmd.Prepare();
            await cmd.ExecuteNonQueryAsync();

            //set values of output parameters
            if ((Parameters != null))
            {
                for (int i = 1; i <= cmd.Parameters.Count - 1; i++)
                {
                    if (cmd.Parameters[i].Direction != ParameterDirection.Input)
                    {
                        ((MySqlParameter)Parameters[i]).Value = cmd.Parameters[i].Value;
                        if (cmd.Parameters[i].ParameterName == "@RETURN_VALUE")
                            returnValue = (long)cmd.Parameters[i].Value;
                    }
                }
            }

            cmd.Dispose();

            return returnValue;
        }








        public DbDataReader GetDataReader(OpenOrmDbConnection cnx, string command, CommandType type)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)cnx.Connection;
            cmd.CommandText = command;
            cmd.CommandType = type;
            cmd.Transaction = cnx.Transaction != null ? (MySqlTransaction)cnx.Transaction : null;

            if (Parameters.Count > 0)
            {
                for (int i = 0; i <= Parameters.Count - 1; i++)
                {
                    cmd.Parameters.Add((MySqlParameter)Parameters[i]);
                }

                cmd.Prepare();
            }

            //string rawQuery = GetRawQuery(cmd);

            //cmd.Prepare();
            MySqlDataReader dr = cmd.ExecuteReader();

            //set values of output parameters
            if ((Parameters != null))
            {
                for (int i = 1; i <= cmd.Parameters.Count - 1; i++)
                {
                    if (cmd.Parameters[i].Direction != ParameterDirection.Input)
                    {
                        ((MySqlParameter)Parameters[i]).Value = cmd.Parameters[i].Value;
                    }
                }
            }

            cmd.Dispose();

            return dr;
        }

        public async Task<DbDataReader> GetDataReaderAsync(OpenOrmDbConnection cnx, string command, CommandType type)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)cnx.Connection;
            cmd.CommandText = command;
            cmd.CommandType = type;
            cmd.Transaction = cnx.Transaction != null ? (MySqlTransaction)cnx.Transaction : null;

            if (Parameters.Count > 0)
            {
                for (int i = 0; i <= Parameters.Count - 1; i++)
                {
                    cmd.Parameters.Add((MySqlParameter)Parameters[i]);
                }

                cmd.Prepare();
            }


            //cmd.Prepare();
            MySqlDataReader dr = (MySqlDataReader) await cmd.ExecuteReaderAsync();

            //set values of output parameters
            if ((Parameters != null))
            {
                for (int i = 1; i <= cmd.Parameters.Count - 1; i++)
                {
                    if (cmd.Parameters[i].Direction != ParameterDirection.Input)
                    {
                        ((MySqlParameter)Parameters[i]).Value = cmd.Parameters[i].Value;
                    }
                }
            }

            cmd.Dispose();

            return dr;
        }

        #endregion

        #region IDisposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés)
                }

                // TODO: libérer les ressources non managées (objets non managés) et substituer le finaliseur
                // TODO: affecter aux grands champs une valeur null
                disposedValue = true;
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~MySqlConnector()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
