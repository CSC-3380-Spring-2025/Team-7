#include <sys/stat.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define KEY 157

long getFileSize(FILE* fp)
{
    long restore = ftell(fp);
    fseek(fp, 0, SEEK_END);
    long result = ftell(fp);
    fseek(fp, restore, SEEK_SET);
    return result;
}

void decryptR(FILE *inputF, FILE *outputF, const char *key)
{
  size_t length = strlen(key);
  size_t z = 0;
  int byte;
  while (fread(&byte, sizeof(byte), 1, inputF) == 1) 
    {
        byte ^= key[z % length];
        fwrite(&byte, sizeof(byte), 1, outputF);
        z++;
    }
}

int main(int argc, char *argv[]) {
FILE *file = fopen(argv[1],"r");
if (file==NULL) 
{
    perror("Error in opening the file");
    return 1;
}

char buffer[4096];
size_t bytesRead;

while((bytesRead = fread(buffer, 1, sizeof(buffer), file))>0)
{
    fwrite(buffer, 1, bytesRead, stdout);
}

const char *key = "157";

FILE *inputF = fopen(argv[1], "rb");
FILE *outputF = fopen(argv[2], "wb");
if (outputF == NULL)
{
    perror("Error in output file");
    fclose(inputF);
    return 1;
}

decryptR(inputF, outputF, key);
fclose(inputF);
fclose(outputF);

long size = getFileSize(file);
printf("\nFile size is : %1d bytes\n ", size);

fclose(file);
return 0;
}
