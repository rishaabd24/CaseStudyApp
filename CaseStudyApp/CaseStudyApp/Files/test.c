#include <stdio.h>
#include <cuda.h>
#include <math.h>

	void
	printmatrix(int *p, int m, int n)
{
	for (int i = 0; i < m; i++)
	{
		for (int j = 0; j < n; j++)
		{
			printf("%d\t", *(p + i * n + j));
		}
		printf("\n");
	}
}
_host_ int isFib(int a)
{
	int x = 5 * a * a - 4;
	int y = 5 * a * a + 4;

	int m = (int)sqrt(x);
	int n = (int)sqrt(y);

	return (x == m || y == n);
}
_global_ void matKernel(int *a, int *c)
{
	int j = threadIdx.x;
	int i = threadIdx.y;
	// printf("i=%d, j=%d \n",i,j);
	int m = blockDim.y;
	int n = blockDim.x;

	if (i == 0 || j == 0 || j == n - 1 || i == m - 1)
	{
		*(c + i * n + j) = 0;
	}
	else
	{
		int k,l;
		int cc;
		cc=0;
		for (k = i - 1; k = < i + 1; k++)
		{
			for (l = j - 1; l <= j + 1; l++)
			{
				if (k == i || l == j)
				{
					continue;
				}
				if (isFib(*(a + k * n + l)) == 1)
				{
					cc=cc+1;
				}
			}
		}
		*(c + i * n + j) = cc;
	}
}

void matFunc(int *a, int *c, int m, int n)
{
	int *d_A, *d_C;
	int size = m * n * sizeof(int);
	cudaMalloc((void **)&d_A, size);
	// cudaMalloc((void **)&d_B, size);
	cudaMalloc((void **)&d_C, size);
	cudaMemcpy(d_A, a, size, cudaMemcpyHostToDevice);
	// cudaMemcpy(d_B, b, size, cudaMemcpyHostToDevice);
	cudaMemcpy(d_C, c, size, cudaMemcpyHostToDevice);
	dim3 dimBlock(n, m);
	matKernel<<<1, dimBlock>>>(d_A, d_C);

	cudaMemcpy(c, d_C, size, cudaMemcpyDeviceToHost);

	printmatrix(c, m, n);
}

int main()
{
	int *a, *c;
	int n = 3, m = 4;

	int size = m * n * sizeof(float);

	a = (int *)malloc(size);
	//    b = (float *)malloc(size)
	c = (int *)malloc(size);

	int i,
		j, k = 1;

	for (i = 0; i < m; i++)
	{
		for (j = 0; j < n; j++)
		{
			*(a + i * n + j) = int(k);
			// *(b + i * n + j) = int(k + 9);
			k = k + 1;
		}
	}

	matFunc(a, c, m, n);
}
