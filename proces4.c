#define WIN32_LEAN_AND_MEAN  /* wymagane przez xmlrpc-c/server_abyss.h */

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <xmlrpc-c/base.h>
#include <xmlrpc-c/server.h>
#include <xmlrpc-c/server_abyss.h>
//======================================================================
static xmlrpc_value * sample_add(
			xmlrpc_env *   const envP,
			xmlrpc_value * const paramArrayP,
			void *         const serverInfo,
			void *         const channelInfo) {

    char *msg;

    /* Parsujemy tablice z argumentami ********************************/
    /* ("ii") oznacza format danych, tutaj: 2 x int *******************/
    
    xmlrpc_decompose_value( envP, paramArrayP, "(s)", &msg);
    if (envP->fault_occurred){
		printf( "error\n");
        return NULL;
	}
	if (msg[0] == ' ') msg++;
	char hex[1024];
	for(int i = 0 ,j =0 ;i < strlen(msg); ++i, j+=2)
		sprintf(hex+j,"%02x",msg[i] & 0xff);
	printf("Wiadomosc: %s \n",hex);
 
    /* Zwracamy wynik *************************************************/
    return xmlrpc_build_value(envP, "i", 1);
}

//======================================================================
int main( void ){

	/* zmienne predefiniowane *****************************************/
	int  port   = 12345;
	//------------------------------------------------------------------
    struct xmlrpc_method_info3 const methodInfo = {
        /* .methodName     = */ "sample.read",
        /* .methodFunction = */ &sample_add,
    };
    
	/* zmienne niezainicjowane ****************************************/
    xmlrpc_server_abyss_parms serverparm;
    xmlrpc_registry * registryP;
    xmlrpc_env env;

    /* inicjujemy serwer **********************************************/
    xmlrpc_env_init(&env);

	/* rejestrujemy zmienne srodowiskowe ******************************/
    registryP = xmlrpc_registry_new(&env);
    if (env.fault_occurred){
        printf( "xmlrpc_registry_new(): %s\n", env.fault_string);
        exit(1);
    }

	/* rejestrujemy nasza metode/funkcje ******************************/
    xmlrpc_registry_add_method3( &env, registryP, &methodInfo );
    if (env.fault_occurred) {
        printf( "xmlrpc_registry_add_method3(): %s\n", env.fault_string );
        exit(1);
    }

	/* ustawiamy parametry serwera ************************************/
    serverparm.config_file_name = NULL;
    serverparm.registryP        = registryP;
    serverparm.port_number      = port;
    serverparm.log_file_name    = "/tmp/xmlrpc_log";
	printf( "server run\n");
	/* uruchamiamy serwer *********************************************/
    xmlrpc_server_abyss(&env, &serverparm, XMLRPC_APSIZE(log_file_name));
    if( env.fault_occurred ){
        printf("xmlrpc_server_abyss(): %s\n", env.fault_string);
        exit(1);
    }

	/* konczymy dzialanie *********************************************/
    return 0;
}
//======================================================================




